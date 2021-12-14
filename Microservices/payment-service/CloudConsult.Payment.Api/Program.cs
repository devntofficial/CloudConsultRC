using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration as IConfiguration;

    // Serilog setup
    builder.Host
        .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Add services to the container.
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

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("PaymentServicePolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.MapControllers();
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