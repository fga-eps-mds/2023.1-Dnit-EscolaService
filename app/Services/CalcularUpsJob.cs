using api;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using service.Interfaces;

namespace app.Services
{
    public class CalcularUpsJob : ICalcularUpsJob
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly AppDbContext dbContext;

        public CalcularUpsJob(IEscolaRepositorio escolaRepositorio, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.escolaRepositorio = escolaRepositorio;
        }

        public async Task ExecutarAsync(PesquisaEscolaFiltro filtro, Ranque novoRanque)
        {
            var raio = 2.0D;
            var desde = 2019;

            var lista = await escolaRepositorio.ListarPaginadaAsync(filtro);
            var upss = await RequisicaoCalcularUps(lista.Items, raio, desde);
            var ranqueEscolas = new EscolaRanque[lista.Items.Count];

            for (int i = 0; i < lista.Items.Count; i++)
            {
                lista.Items[i].Ups = upss[i];
                ranqueEscolas[i] = new()
                {
                    Pontuacao = upss[i],
                    RanqueId = novoRanque.Id,
                    EscolaId = lista.Items[i].Id,
                };
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task<List<int>> RequisicaoCalcularUps(List<Escola> escolas, double raioKm, int desdeAno)
        {
            var localizacoes = escolas.Select(
                e => new LocalizacaoEscola
                {
                    // As latitudes no banco são armazenadas com vírgula em vez de ponto.
                    Latitude = double.Parse(e.Latitude.Replace(',', '.')),
                    Longitude = double.Parse(e.Longitude.Replace(',', '.')),
                });

            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10)
            };

            var conteudo = JsonContent.Create(localizacoes);

            var resposta = await client.PostAsync(
                $"http://localhost:7085/api/calcular/ups/escolas?desde={desdeAno}&raiokm={raioKm}",
                conteudo);

            resposta.EnsureSuccessStatusCode();

            var upss = await resposta.Content.ReadFromJsonAsync<List<int>>()
                ?? throw new ApiException(ErrorCodes.Unknown);

            return upss;
        }
    }
}