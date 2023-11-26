using service.Interfaces;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Web;
using api.Escolas;
using app.Repositorios.Interfaces;
using app.Entidades;

namespace app.Services
{
    public class SolicitacaoAcaoService : ISolicitacaoAcaoService
    {
        private readonly ISmtpClientWrapper _smtpClientWrapper;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly ISolicitacaoAcaoRepositorio solicitacaoAcaoRepositorio;
        private readonly AppDbContext dbContext;

        public SolicitacaoAcaoService(AppDbContext dbContext, ISmtpClientWrapper smtpClientWrapper, IHttpClientFactory httpClientFactory, IConfiguration configuration, ISolicitacaoAcaoRepositorio solicitacaoAcaoRepositorio)
        {
            this.dbContext = dbContext;
            _smtpClientWrapper = smtpClientWrapper;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.solicitacaoAcaoRepositorio = solicitacaoAcaoRepositorio;
        }

        public void EnviarSolicitacaoAcao(SolicitacaoAcaoData solicitacaoAcaoDTO)
        {
            string ciclosEnsino = "\n" + string.Join("\n", solicitacaoAcaoDTO.CiclosEnsino.Select(ciclo => $"    > {ciclo}"));

            string mensagem = $"Nova solicitação de ação em escola.\n\n" +
                            $"Escola: {solicitacaoAcaoDTO.Escola}\n" +
                            $"UF: {solicitacaoAcaoDTO.UF}\n" +
                            $"Municipio: {solicitacaoAcaoDTO.Municipio}\n" +
                            $"Nome do Solicitante: {solicitacaoAcaoDTO.NomeSolicitante}\n" +
                            $"Vínculo com a escola: {solicitacaoAcaoDTO.VinculoEscola}\n" +
                            $"Email: {solicitacaoAcaoDTO.Email}\n" +
                            $"Telefone: {solicitacaoAcaoDTO.Telefone}\n" +
                            $"Ciclos de ensino: {ciclosEnsino}\n" +
                            $"Quantidade de alunos: {solicitacaoAcaoDTO.QuantidadeAlunos}\n" +
                            $"Observações: {solicitacaoAcaoDTO.Observacoes}\n";
            var emailDestinatario = Environment.GetEnvironmentVariable("EMAIL_DNIT") ?? "";
            EnviarEmail(emailDestinatario, "Solicitação de Serviço", mensagem);
        }

        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS")!;

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            _smtpClientWrapper.Send(mensagem);
        }

        public async Task Criar(SolicitacaoAcaoData solicitacao)
        {
            // TODO: E se a escola não existir?
            var solicitacaoExistente = solicitacaoAcaoRepositorio.ObterPorEscolaIdAsync(solicitacao.EscolaId)
                ?? throw new Exception("Já foi feita uma solicitação para essa escola");

            await solicitacaoAcaoRepositorio.Criar(solicitacao);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EscolaInep>> ObterEscolas(int municipio)
        {
            var uriBuilder = new UriBuilder(configuration["ApiInepUrl"]!);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["cidade"] = municipio.ToString();

            uriBuilder.Query = query.ToString();
            string url = uriBuilder.ToString();

            var httpClient = this.httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string conteudo = await response.Content.ReadAsStringAsync();

            var jArray = JArray.Parse(conteudo);
            var escolas = jArray[1].ToObject<IEnumerable<EscolaInep>>();


            return escolas;
        }
    }
}
