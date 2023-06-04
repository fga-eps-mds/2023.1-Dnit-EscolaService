using AutoMapper;
using dominio;
using repositorio;
using service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class SolicitacaoAcaoService : ISolicitacaoAcaoService
    {
        private readonly IEmailService emailService;
        public SolicitacaoAcaoService(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        public void EnviarSolicitacaoAcao(SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            emailService.EnviarEmail("dnit@gmail.com", "Solicitação de Serviço", "");


        }
    }
}
