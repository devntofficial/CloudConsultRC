using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using CloudConsult.Consultation.Infrastructure.Consumers;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
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

    // Serilog setup with Elasticsearch
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
    builder.Services.AddCommonJwtAuthentication(config);
    builder.Services.AddCommonMediatorConfiguration("CloudConsult.Consultation.Domain", "CloudConsult.Consultation.Infrastructure");
    builder.Services.AddCommonValidationsFrom("CloudConsult.Consultation.Domain");
    builder.Services.AddCommonKafkaProducer(config);
    builder.Services.AddCommonMiddlewares();
    builder.Services.AddHostedService<ReportUploadedConsumer>();
    builder.Services.AddHostedService<PaymentAcceptedConsumer>();
    builder.Services.AddHostedService<PaymentRejectedConsumer>();

    var app = builder.Build();

    //migrate database
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<ConsultationDbContext>();
        dataContext.Database.Migrate();
    }

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
                    $"Cloud Consult - Consultation API Reference {description.GroupName}");
        });
    }
    
    app.UseCors("ConsultationServicePolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    
    app.UseMvc();
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Consultation Service - Shut down complete");
    Log.CloseAndFlush();
}