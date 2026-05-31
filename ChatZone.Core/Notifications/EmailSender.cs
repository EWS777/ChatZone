using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace ChatZone.Core.Notifications;

public class EmailSender(IOptions<SmtpSettings> options)
{
    private readonly SmtpSettings _settings = options.Value;

    public Task SendCodeToEmail(string email, string token, CancellationToken cancellationToken)
    {
        string linkToClick = $"{_settings.LinkToClick}confirm?link={token}";
        string subject = "Hello! Please confirm the registration";
        string title = "Activate Your Profile";

        return SendEmailAsync(email, subject, title, linkToClick, cancellationToken);
    }

    public Task ResetPassword(string email, string token, CancellationToken cancellationToken)
    {
        string linkToClick = $"{_settings.LinkToClick}reset?link={token}";
        string subject = "Hello! Password Reset Request";
        string title = "Reset Your Password";

        return SendEmailAsync(email, subject, title, linkToClick, cancellationToken);
    }

    private async Task SendEmailAsync(string email, string subject, string title, string linkToClick, CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient(_settings.SMTPServer, _settings.SMTPPort);
        smtpClient.Credentials = new NetworkCredential(_settings.SMTPEmail, _settings.SMTPPassword);
        smtpClient.EnableSsl = true;

        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_settings.SMTPEmail);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = GenerateHtmlBody(title, linkToClick);

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }

    private string GenerateHtmlBody(string title, string linkToClick)
    {
        return $@"
        <html>
        <head>
            <style>
                .container {{ font-family: Arial, sans-serif; text-align: center; background-color: #f4f4f4; padding: 20px; border-radius: 10px; width: 80%; margin: auto; }}
                .button {{ display: inline-block; padding: 10px 20px; font-size: 16px; color: #fff; background-color: #28a745; text-decoration: none; border-radius: 5px; font-weight: bold; margin-top: 20px; }}
                .button:hover {{ background-color: #218838; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>{title}</h2>
                <p>Please click the button below to proceed:</p>
                <a href='{linkToClick}' class='button'>Click Here</a>
            </div>
        </body>
        </html>";
    }
}