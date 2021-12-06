using CloudConsult.Common.Configurations;
using MailKit.Net.Smtp;
using MimeKit;

namespace CloudConsult.Common.Email;

public class EmailService : IEmailService
{
    private readonly EmailServiceConfiguration _config;

    public EmailService(EmailServiceConfiguration config)
    {
        _config = config;
    }

    public async Task SendTextMail(EmailServiceTextParameters param, SmtpClient emailClient)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(param.FromDisplayName ?? "Cloud Consult", param.FromEmail ?? _config.Username));
        message.To.Add(new MailboxAddress(param.ToDisplayName, param.ToEmail));
        message.Subject = param.Subject;

        message.Body = new TextPart("plain")
        {
            Text = param.Message
        };

        await emailClient.ConnectAsync(_config.HostName, _config.Port, _config.UseSSL);
        await emailClient.AuthenticateAsync(_config.Username, _config.Password);
        await emailClient.SendAsync(message);
        await emailClient.DisconnectAsync(true);
    }
}