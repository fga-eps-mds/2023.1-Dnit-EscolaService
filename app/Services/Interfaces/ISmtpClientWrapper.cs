using System.Net.Mail;

namespace service.Interfaces
{
    public interface ISmtpClientWrapper
    {
        void Send(MailMessage message);
    }
}
