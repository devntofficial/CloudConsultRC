using CloudConsult.Common.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
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
    builder.Services.AddCommonEmailService(config);
    
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
                    $"Cloud Consult - Notification API Reference {description.GroupName}");
        });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("NotificationServicePolicy");

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Notification Service - Shut down complete");
    Log.CloseAndFlush();
}