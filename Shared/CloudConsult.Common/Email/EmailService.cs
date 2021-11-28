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

    public async Task SendTextMail(EmailServiceTextParameters param)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(param.FromDisplayName, param.FromEmail));
        message.To.Add(new MailboxAddress(param.ToDisplayName, param.ToEmail));
        message.Subject = param.Subject;

        message.Body = new TextPart("plain")
        {
            Text = param.Message
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config.HostName, _config.Port, _config.UseSSL);
        await client.AuthenticateAsync(_config.Username, _config.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}