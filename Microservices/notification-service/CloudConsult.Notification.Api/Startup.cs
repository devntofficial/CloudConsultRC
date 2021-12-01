using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudConsult.Notification.Common.Configuration;
using CloudConsult.Notification.Infrastructure.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Notification.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            KafkaConsumerConfiguration kafkaConsumerConfiguration = new KafkaConsumerConfiguration();
            _configuration.Bind(nameof(KafkaConsumerConfiguration), kafkaConsumerConfiguration);
            services.AddHostedService<DoctorUpdateConsumer>();
            services.AddSingleton(kafkaConsumerConfiguration);
            services.AddHostedService<DoctorUpdateConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Cloud Consult Notification service is up and running!"); });
            });
        }
    }
}