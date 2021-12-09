using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
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
    builder.Services.AddCommonHashingService();
    builder.Services.AddCommonSwaggerDocs(config);
    builder.Services.AddCommonApiVersioning();
    builder.Services.AddCommonJwtAuthentication(config);
    builder.Services.AddCommonMediatorConfiguration("CloudConsult.Identity.Domain", "CloudConsult.Identity.Infrastructure");
    builder.Services.AddCommonValidationsFrom("CloudConsult.Identity.Domain");
    builder.Services.AddCommonKafkaProducer(config);
    builder.Services.AddCommonMiddlewares();

    var app = builder.Build();

    //migrate database
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
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
                    $"Cloud Consult - Identity API Reference {description.GroupName}");
        });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("IdentityServicePolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.MapControllers();
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
    Log.Information("Identity Service - Shut down complete");
    Log.CloseAndFlush();
}