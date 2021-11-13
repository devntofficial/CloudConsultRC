using CloudConsult.Common.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration as IConfiguration;

// Add services to the container.
builder.Services.AddCommonExtensionsFromCurrentAssembly(config);
builder.Services.AddCommonSwaggerDocs(config);
builder.Services.AddCommonApiVersioning();
builder.Services.AddCommonJwtAuthentication(config);
builder.Services.AddCommonMediatorConfiguration("CloudConsult.Member.Domain", "CloudConsult.Member.Infrastructure");
builder.Services.AddCommonValidationsFrom("CloudConsult.Member.Domain");
builder.Services.AddCommonKafkaProducer(config);

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
                $"Cloud Consult - Member API Reference {description.GroupName}");
    });
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MemberServicePolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
