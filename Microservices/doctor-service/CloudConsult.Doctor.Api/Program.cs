using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
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
    builder.Services.AddCommonJwtAuthentication(config);
    builder.Services.AddCommonMediatorConfiguration("CloudConsult.Doctor.Domain", "CloudConsult.Doctor.Infrastructure");
    builder.Services.AddCommonValidationsFrom("CloudConsult.Doctor.Domain");
    builder.Services.AddCommonKafkaProducer(config);
    builder.Services.AddCommonMiddlewares();

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
                    $"Cloud Consult - Doctor API Reference {description.GroupName}");
        });
    }
    
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("DoctorServicePolicy");
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
    Log.Information("Doctor Service - Shut down complete");
    Log.CloseAndFlush();
}