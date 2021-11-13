using CloudConsult.Common.DependencyInjection;
using CloudConsult.Identity.Api.Extensions;
using CloudConsult.Identity.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudConsult.Identity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureExtensionsFromAssemblyContaining<ApiExtension>(Configuration)
                .AddCommonApiVersioning()
                .AddCommonHashingService()
                .AddCommonSwaggerDocs(Configuration)
                .AddCommonJwtAuthentication(Configuration)
                .AddCommonMediatorConfiguration("CloudConsult.Identity.Domain", "CloudConsult.Identity.Infrastructure")
                .AddCommonValidationsFrom("CloudConsult.Identity.Domain")
                .AddCommonKafkaProducer(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api-docs";
                    foreach (var description in versionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/api-docs/{description.GroupName}/docs.json",
                            $"Cloud Consult - Identity API Reference {description.GroupName}");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("IdentityServicePolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            //register custom middlewares here and only here, order of middleware matters in execution pipeline
            app.UseMiddleware<UnhandledExceptionMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
