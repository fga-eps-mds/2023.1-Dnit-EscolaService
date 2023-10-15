﻿using service.Interfaces;
using System.Net;
using System.Net.Mail;

namespace app.Services
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient smtpClient;

        public SmtpClientWrapper()
        {
            string emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS");
            string senhaRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_PASSWORD");

            smtpClient = new SmtpClient("smtp-mail.outlook.com")
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
