using CloudConsult.Common.DependencyInjection;
using CloudConsult.Common.Middlewares;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
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
        if (!dataContext.Roles.Any())
        {
            await dataContext.Roles.AddRangeAsync(new Role[]
            {
                new Role { RoleName = "Administrator", Timestamp = DateTime.Now },
                new Role { RoleName = "Doctor", Timestamp = DateTime.Now },
                new Role { RoleName = "Member", Timestamp = DateTime.Now }
            });

            await dataContext.SaveChangesAsync();
        }
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

    //app.UseHttpsRedirection();
    app.UseCors("IdentityServicePolicy");
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