using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ChatZone.Core.Notifications;

public class EmailSender
{
    private static IConfiguration _configuration;

    public static void EmailSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static async Task SendCodeToEmail(string email, string token, CancellationToken cancellationToken)
    {
        string linkToClick = _configuration["Gmail:LinkToClick"] + "confirm?link=" + token;
        
        using (var smtpClient = new SmtpClient(_configuration["Gmail:SMTPServer"], Convert.ToInt32(_configuration["Gmail:SMTPPort"])))
        {
            smtpClient.Credentials = new NetworkCredential(_configuration["Gmail:SMTPEmail"], _configuration["Gmail:SMTPPassword"]);

            smtpClient.EnableSsl = true;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_configuration["Gmail:SMTPEmail"]);
                mailMessage.To.Add(email);
                mailMessage.Subject = "Hello! Please confirm the registration";
                mailMessage.IsBodyHtml = true;

                mailMessage.Body = $@"
                <html>
                <head>
                    <style>
                        .container {{
                            font-family: Arial, sans-serif;
                            text-align: center;
                            background-color: #f4f4f4;
                            padding: 20px;
                            border-radius: 10px;
                            width: 80%;
                            margin: auto;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            font-size: 16px;
                            color: #fff;
                            background-color: #28a745;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                            margin-top: 20px;
                        }}
                        .button:hover {{
                            background-color: #218838;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Activate Your Profile</h2>
                        <p>Please click the button below to activate your account:</p>
                        <a href='{linkToClick}' class='button'>Activate</a>
                    </div>
                </body>
                </html>";
                try
                {
                    await smtpClient.SendMailAsync(mailMessage, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }
    }
    
    public static async Task ResetPassword(string email, string token, CancellationToken cancellationToken)
    {
        string linkToClick = _configuration["Gmail:LinkToClick"] + "reset?link=" + token;
        
        using (var smtpClient = new SmtpClient(_configuration["Gmail:SMTPServer"], Convert.ToInt32(_configuration["Gmail:SMTPPort"])))
        {
            smtpClient.Credentials = new NetworkCredential(_configuration["Gmail:SMTPEmail"], _configuration["Gmail:SMTPPassword"]);

            smtpClient.EnableSsl = true;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_configuration["Gmail:SMTPEmail"]);
                mailMessage.To.Add(email);
                mailMessage.Subject = "Hello! Please confirm the registration";
                mailMessage.IsBodyHtml = true;

                mailMessage.Body = $@"
                <html>
                <head>
                    <style>
                        .container {{
                            font-family: Arial, sans-serif;
                            text-align: center;
                            background-color: #f4f4f4;
                            padding: 20px;
                            border-radius: 10px;
                            width: 80%;
                            margin: auto;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            font-size: 16px;
                            color: #fff;
                            background-color: #28a745;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                            margin-top: 20px;
                        }}
                        .button:hover {{
                            background-color: #218838;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Activate Your Profile</h2>
                        <p>Please click the button below to activate your account:</p>
                        <a href='{linkToClick}' class='button'>Activate</a>
                    </div>
                </body>
                </html>";
                try
                {
                    await smtpClient.SendMailAsync(mailMessage, cancellationToken);
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