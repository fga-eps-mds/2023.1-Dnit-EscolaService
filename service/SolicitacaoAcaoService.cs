using AutoMapper;
using dominio;
using repositorio;
using service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web;
using Microsoft.Extensions.Http;

namespace service
{
    public class SolicitacaoAcaoService : ISolicitacaoAcaoService
    {
        private readonly ISmtpClientWrapper _smtpClientWrapper;
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitacaoAcaoService(ISmtpClientWrapper smtpClientWrapper, IHttpClientFactory httpClientFactory)
        {
            _smtpClientWrapper = smtpClientWrapper;
            this.httpClientFactory = httpClientFactory;
        }

        public void EnviarSolicitacaoAcao(SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            string ciclosEnsino = "\n" + string.Join("\n", solicitacaoAcaoDTO.CiclosEnsino.Select(ciclo => $"    > {ciclo}"));

            string mensagem = $"Nova solicitação de ação em escola.\n\n" +
                            $"Escola: {solicitacaoAcaoDTO.Escola}\n" +
                            $"Nome do Solicitante: {solicitacaoAcaoDTO.NomeSolicitante}\n" +
                            $"Vínculo com a escola: {solicitacaoAcaoDTO.VinculoEscola}\n" +
                            $"Email: {solicitacaoAcaoDTO.Email}\n" +
                            $"Telefone: {solicitacaoAcaoDTO.Telefone}\n" +
                            $"Ciclos de ensino: {ciclosEnsino}\n" +
                            $"Quantidade de alunos: {solicitacaoAcaoDTO.QuantidadeAlunos}\n" +
                            $"Observações: {solicitacaoAcaoDTO.Observacoes}\n";
            string emailDestinatario = Environment.GetEnvironmentVariable("EMAIL_DNIT");
            EnviarEmail(emailDestinatario, "Solicitação de Serviço", mensagem);
        }
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS");

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            _smtpClientWrapper.Send(mensagem);
        }

        public async Task<IEnumerable<EscolaInep>> ObterEscolas(string nome, string estado)
        {
            var uriBuilder = new UriBuilder("http://educacao.dadosabertosbr.com/api/escolas/buscaavancada");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["nome"] = nome;
            if (estado != null)
            {
                query["estado"] = estado;
            }
           
            uriBuilder.Query = query.ToString();
            string url = uriBuilder.ToString();

            var httpClient = this.httpClient.CreateClient();

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string conteudo = await response.Content.ReadAsStringAsync();

            var jArray = JArray.Parse(conteudo);
            var escolas = jArray[1].ToObject<IEnumerable<EscolaInep>>();

            return escolas;
        }
    }
}
