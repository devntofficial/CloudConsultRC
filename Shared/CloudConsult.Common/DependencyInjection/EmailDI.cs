using CloudConsult.Common.Configurations;
using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Common.DependencyInjection
{
    public static class EmailDI
    {
        public static void AddCommonEmailService(this IServiceCollection services, IConfiguration configuration, string templatePath)
        {
            var emailConfig = new EmailServiceConfiguration();
            configuration.Bind(nameof(EmailServiceConfiguration), emailConfig);
            services.AddSingleton(emailConfig);

            services
                .AddFluentEmail(emailConfig.Username, "Cloud Consult")
                .AddRazorRenderer(templatePath)
                .AddMailKitSender(new SmtpClientOptions
                {
                    Server = emailConfig.HostName,
                    Port = emailConfig.Port,
                    User = emailConfig.Username,
                    Password = emailConfig.Password,
                    UseSsl = emailConfig.UseSSL
                });
        }
    }
}
