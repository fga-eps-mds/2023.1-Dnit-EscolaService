﻿using service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace app.service
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
