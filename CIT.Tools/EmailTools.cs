using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public class EmailTools
    {
        private readonly IConfiguration _configuration;
        public EmailTools(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailWithInfoAsync(string userEmail, string subject, string body)
        {
            string appEmail = _configuration["EmailAccount:AppEmail"];
            string appEmailPassword = _configuration["EmailAccount:AppEmailPassword"];

            int.TryParse(_configuration["EmailAccount:port"], out int port);
            SmtpClient smtpClient = new SmtpClient(_configuration["EmailAccount:smtp"], port);
            smtpClient.Credentials = new NetworkCredential(appEmail, appEmailPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(appEmail, "Sistema Integral de Préstamos");
            mailMessage.To.Add(new MailAddress(userEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
