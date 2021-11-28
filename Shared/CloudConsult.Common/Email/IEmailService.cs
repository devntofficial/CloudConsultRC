namespace CloudConsult.Common.Email;

public interface IEmailService
{
    Task SendTextMail(EmailServiceTextParameters param);
}