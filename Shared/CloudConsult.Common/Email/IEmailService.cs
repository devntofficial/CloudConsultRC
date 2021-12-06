using MailKit.Net.Smtp;

namespace CloudConsult.Common.Email;

public interface IEmailService
{
    Task SendTextMail(EmailServiceTextParameters param, SmtpClient emailClient);
}