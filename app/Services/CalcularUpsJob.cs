using api;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using Hangfire;
using Microsoft.Extensions.Options;
using service.Interfaces;

namespace app.Services
{
    public class CalcularUpsJob : ICalcularUpsJob
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly IRanqueRepositorio ranqueRepositorio;
        private readonly AppDbContext dbContext;
        private readonly string UpsServiceHost;
        private static readonly string Endpoint = "api/calcular/ups/escolas";

        public CalcularUpsJob(
            IEscolaRepositorio escolaRepositorio,
            IRanqueRepositorio ranqueRepositorio,
            IOptions<UpsServiceConfig> upsServiceConfig,
            AppDbContext dbContext
        )
        {
            this.dbContext = dbContext;
            this.escolaRepositorio = escolaRepositorio;
            this.ranqueRepositorio = ranqueRepositorio;
            this.UpsServiceHost = upsServiceConfig.Value.Host;
        }

        [MaximumConcurrentExecutions(3, timeoutInSeconds: 20 * 60)]
        public async Task ExecutarAsync(PesquisaEscolaFiltro filtro, int novoRanqueId, int timeoutMinutos)
        {
            var raio = 2.0D;
            var desde = 2019;

            var lista = await escolaRepositorio.ListarPaginadaAsync(filtro);
            var upss = await RequisicaoCalcularUps(lista.Items, raio, desde, timeoutMinutos);
            var ranqueEscolas = new EscolaRanque[lista.Items.Count];

            for (int i = 0; i < lista.Items.Count; i++)
            {
                lista.Items[i].Ups = upss[i];
                ranqueEscolas[i] = new()
                {
                    Pontuacao = upss[i],
                    RanqueId = novoRanqueId,
                    EscolaId = lista.Items[i].Id,
                };
            }

            await dbContext.EscolaRanques.AddRangeAsync(ranqueEscolas);
            await dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue<ICalcularUpsJob>(
                job => job.FinalizarCalcularUpsJob(novoRanqueId));
        }

        private async Task<List<int>> RequisicaoCalcularUps(List<Escola> escolas, double raioKm, int desdeAno, int timeoutMinutos)
        {
            var localizacoes = escolas.Select(
                e => new LocalizacaoEscola
                {
                    // As latitudes no banco são armazenadas com vírgula em vez de ponto.
                    Latitude = double.Parse(e.Latitude.Replace(',', '.')),
                    Longitude = double.Parse(e.Longitude.Replace(',', '.')),
                });

            // TODO: Autenticar e autorizar esse cliente com a permissão
            // de /calcular/ups/escolas.
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(timeoutMinutos)
            };

            var conteudo = JsonContent.Create(localizacoes);

            var resposta = await client.PostAsync(
                UpsServiceHost + Endpoint + $"?desde={desdeAno}&raiokm={raioKm}",
                // $"http://localhost:7085/api/calcular/ups/escolas?desde={desdeAno}&raiokm={raioKm}",
                conteudo);

            resposta.EnsureSuccessStatusCode();

            var upss = await resposta.Content.ReadFromJsonAsync<List<int>>()
                ?? throw new ApiException(ErrorCodes.Unknown);

            return upss;
        }

        // Não pode ser mais que 1. Essa limitação serve como um Mutex
        [MaximumConcurrentExecutions(1)]
        public async Task FinalizarCalcularUpsJob(int ranqueId)
        {
            var ranqueEmProgresso = (await ranqueRepositorio.ObterPorIdAsync(ranqueId))!;
            if (ranqueEmProgresso.DataFim != null || ranqueEmProgresso.BateladasEmProgresso == 0)
                return;

            ranqueEmProgresso.BateladasEmProgresso--;

            if (ranqueEmProgresso.BateladasEmProgresso == 0)
                ranqueEmProgresso.DataFim = DateTimeOffset.Now;

            await dbContext.SaveChangesAsync();
        }
    }
}