using Microsoft.EntityFrameworkCore;

using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using service.Interfaces;
using Hangfire;

namespace app.Services
{
    public class RanqueService : IRanqueService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly AppDbContext dbContext;

        public RanqueService(IEscolaRepositorio escolaRepositorio, AppDbContext dbContext)
        {
            this.escolaRepositorio = escolaRepositorio;
            this.dbContext = dbContext;
        }

        public async Task CalcularNovoRanqueAsync()
        {
            var tamanhoBatelada = 5;
            var totalEscolas = dbContext.Escolas.CountAsync();
            var novoRanque = new Ranque
            {
                DataInicio = DateTimeOffset.Now,
            };
            dbContext.Ranques.Add(novoRanque);
            await dbContext.SaveChangesAsync();

            var filtro = new PesquisaEscolaFiltro { Pagina = 1, TamanhoPagina = tamanhoBatelada };
            BackgroundJob.Enqueue(() => ExecutarJobAsync(filtro, novoRanque));

            novoRanque.DataFim = DateTimeOffset.Now;
            await dbContext.SaveChangesAsync();
        }

        public async Task ExecutarJobAsync(PesquisaEscolaFiltro filtro, Ranque novoRanque)
        {
            var lista = await escolaRepositorio.ListarPaginadaAsync(filtro);
            var escolaLocalizacoes = lista.Items.Select(
                e => new LocalizacaoEscola
                {
                    // As latitudes no banco são armazenadas com vírgula em vez de ponto.
                    Latitude = double.Parse(e.Latitude.Replace(',', '.')),
                    Longitude = double.Parse(e.Longitude.Replace(',', '.')),
                });

            var upsClient = new UpsClient();
            var upsCalculados = await upsClient.CalcularUps(escolaLocalizacoes);
            var ranqueEscolas = new EscolaRanque[lista.Items.Count];
            for (int i = 0; i < lista.Items.Count; i++)
            {
                lista.Items[i].Ups = upsCalculados[i];
                ranqueEscolas[i] = new()
                {
                    Pontuacao = upsCalculados[i],
                    RanqueId = novoRanque.Id,
                    EscolaId = lista.Items[i].Id,
                };
            }

            await dbContext.SaveChangesAsync();
        }
    }

    public class LocalizacaoEscola
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}