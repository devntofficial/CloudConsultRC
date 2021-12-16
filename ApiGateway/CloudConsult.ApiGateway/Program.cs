using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
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
    s.AddOcelot();
}).ConfigureLogging((hostingContext, logging) =>
{
    logging.AddConsole();
});
var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
