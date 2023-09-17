using service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient smtpClient;

        public SmtpClientWrapper()
        {
            var emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS");
            var senhaRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_PASSWORD");
            var smtpDomain = Environment.GetEnvironmentVariable("EMAIL_SERVICE_SMTP") ?? "smtp-mail.outlook.com";

            smtpClient = new SmtpClient(smtpDomain)
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,
            };
        }

        public void Send(MailMessage message)
        {
            smtpClient.Send(message);
        }
    }
}
