using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using CloudConsult.Payment.Infrastructure.Clients;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration as IConfiguration;

    // Serilog setup
    var elasticSearchServers = config["ElasticsearchServers"].Split(',').Select(x => new Uri(x));
    builder.Host
        .UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticSearchServers)
        {
            IndexFormat = $"{config["ApiName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now:MM-yyyy}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1
        }));

    //Add Metrics using Prometheus
    builder.Host.UseMetricsWebTracking().UseMetrics(options =>
    {
        options.EndpointOptions = endpointOptions =>
        {
            endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        };
    });

    // Add services to the container.
    builder.Services.AddMetrics();
    builder.Services.AddMvcCore(x => x.EnableEndpointRouting = false).AddMetricsCore();
    builder.Services.AddCommonExtensionsFromCurrentAssembly(config);
    builder.Services.AddCommonSwaggerDocs(config);
    builder.Services.AddCommonApiVersioning();
    builder.Services.AddCommonMediatorConfiguration("CloudConsult.Payment.Domain", "CloudConsult.Payment.Infrastructure");
    builder.Services.AddCommonValidationsFrom("CloudConsult.Payment.Domain");
    builder.Services.AddCommonMiddlewares();
    builder.Services.AddSingleton(x => new ClusterClient(new Configuration
    {
        Seeds = config["KafkaConfiguration:BootstrapServers"],
        RequiredAcks = config["KafkaConfiguration:Acks"] == "All" ? RequiredAcks.AllInSyncReplicas : config["KafkaConfiguration:Acks"] == "Leader" ? RequiredAcks.Leader : RequiredAcks.None,
        MaxRetry = Convert.ToInt32(config["KafkaConfiguration:MessageSendMaxRetries"]),
        RequestTimeoutMs = Convert.ToInt32(config["KafkaConfiguration:MessageTimeoutMs"])
    }, new ConsoleLogger()));
    builder.Services.AddCommonApiClient<ConsultationApiClient>(config["ConsultationApiServer"]);

    var app = builder.Build();
    var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "api-docs";
            foreach (var description in versionProvider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/api-docs/{description.GroupName}/docs.json",
                    $"Cloud Consult - Payment API Reference {description.GroupName}");
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("PaymentServicePolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseMvc();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Payment Service - Shut down complete");
    Log.CloseAndFlush();
}