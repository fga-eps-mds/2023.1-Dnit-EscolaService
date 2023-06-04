using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Interfaces
{
    public interface IEmailService
    {
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo);
    }
}
