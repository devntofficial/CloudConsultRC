using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;
using Prometheus.SystemMetrics;
using Serilog;
using Serilog.Sinks.Elasticsearch;

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

//add gateway configuration
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddOcelot(builder.Environment)
        .AddEnvironmentVariables();
}).ConfigureServices(s =>
{
    s.AddSystemMetrics();
    s.AddOcelot();
})
;
var app = builder.Build();

app.UseRouting();
app.UseHttpMetrics();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
});

app.UseOcelot().Wait();

app.Run();
