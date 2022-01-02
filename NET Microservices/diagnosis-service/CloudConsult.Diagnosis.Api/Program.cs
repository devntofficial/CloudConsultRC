using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using CloudConsult.Diagnosis.Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;
using Prometheus.SystemMetrics;
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


    // Add services to the container.
    builder.Services.AddSystemMetrics();
    builder.Services.AddCommonExtensionsFromCurrentAssembly(config);
    builder.Services.AddCommonSwaggerDocs(config);
    builder.Services.AddCommonApiVersioning();
    builder.Services.AddCommonJwtAuthentication(config);
    builder.Services.AddCommonMediatorConfiguration("CloudConsult.Diagnosis.Domain", "CloudConsult.Diagnosis.Infrastructure");
    builder.Services.AddCommonValidationsFrom("CloudConsult.Diagnosis.Domain");
    builder.Services.AddCommonKafkaProducer(config);
    builder.Services.AddCommonMiddlewares();
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
                    $"Cloud Consult - Diagnosis API Reference {description.GroupName}");
        });
    }

    //app.UseHttpsRedirection();
    app.UseCors("DiagnosisServicePolicy");
    app.UseRouting();
    app.UseHttpMetrics();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics();
        endpoints.MapControllers();
    });
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Diagnosis Service - Shut down complete");
    Log.CloseAndFlush();
}