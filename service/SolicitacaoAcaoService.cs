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
        public SolicitacaoAcaoService()
        {

        }
        public void EnviarSolicitacaoAcao(SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            string mensagem = $"Nova solicitação de ação em escola\n\n" +
                            $"Escola: {solicitacaoAcaoDTO.Escola}\n" +
                            $"Nome do Solicitante: {solicitacaoAcaoDTO.NomeSolicitante}\n" +
                            $"Vínculo com a escola: {solicitacaoAcaoDTO.VinculoEscola}\n" +
                            $"Email: {solicitacaoAcaoDTO.Email}\n" +
                            $"Telefone: {solicitacaoAcaoDTO.Telefone}\n" +
                            $"Ciclos de ensino: {solicitacaoAcaoDTO.CiclosEnsino}\n" +
                            $"Quantidade de alunos{solicitacaoAcaoDTO.QuantidadeAlunos}\n" +
                            $"Observações: {solicitacaoAcaoDTO.Observacoes}\n";
            string emailDestinatario = Environment.GetEnvironmentVariable("EMAIL_DNIT");
            EnviarEmail(emailDestinatario, "Solicitação de Serviço", mensagem);



        }
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS");
            string senhaRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_PASSWORD");

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            var clienteSmtp = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,

            };
            clienteSmtp.Send(mensagem);
        }
    }
}
