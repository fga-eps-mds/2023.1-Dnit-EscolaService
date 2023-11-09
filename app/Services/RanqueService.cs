using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using service.Interfaces;

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
            var tamanhoBatelada = 4;
            var totalEscolas = dbContext.Escolas.CountAsync();

            var novoRanque = new Ranque
            {
                DataInicio = DateTimeOffset.Now,
            };

            // dbContext.Ranques.Add(novoRanque);
            // dbContext.SaveChanges();

            var filtro = new PesquisaEscolaFiltro { Pagina = 1, TamanhoPagina = tamanhoBatelada };
            var escolas = await escolaRepositorio.ListarPaginadaAsync(filtro);
            var escolaLocalizacoes = escolas.Items.Select(
                e => new LocalizacaoEscola
                {
                    // As latitudes no banco são armazenadas com vírgula em vez de ponto.
                    Latitude = double.Parse(e.Latitude.Replace(',', '.')),
                    Longitude = double.Parse(e.Longitude.Replace(',', '.')),
                });

            var client = new UpsClient();
            var upss = await client.CalcularUps(escolaLocalizacoes);

            for (int i = 0; i < escolas.Items.Count; i++)
            {
                // escolas.Items[i].
            }

            novoRanque.DataFim = DateTimeOffset.Now;
            // dbContext.SaveChanges();
        }
    }

    public class LocalizacaoEscola
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}