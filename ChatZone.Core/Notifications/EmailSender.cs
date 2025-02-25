using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ChatZone.Core.Notifications;

public class EmailSender(IConfiguration configuration)
{
    public  void SendCodeToEmail(string message)
    {
        using (var smtpClient = new SmtpClient(configuration["Gmail:SMTPServer"], Convert.ToInt32(configuration["Gmail:SMTPPort"])))
        {
            smtpClient.Credentials =
                new NetworkCredential(configuration["Gmail:SMTPEmail"], configuration["Gmail:SMTPPassword"]);

            smtpClient.EnableSsl = true;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(configuration["Gmail:SMTPEmail"]);
                mailMessage.To.Add(message);
                mailMessage.Subject = "Hello";
                mailMessage.Body = "Hello from Body!";
                try
                {
                    smtpClient.Send(mailMessage);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }
    }
}