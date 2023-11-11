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
            var tamanhoBatelada = 200;
            var totalEscolas = dbContext.Escolas.CountAsync();
            var novoRanque = new Ranque
            {
                DataInicio = DateTimeOffset.Now,
            };
            dbContext.Ranques.Add(novoRanque);
            await dbContext.SaveChangesAsync();

            var filtro = new PesquisaEscolaFiltro { Pagina = 1, TamanhoPagina = tamanhoBatelada };
            BackgroundJob.Enqueue<ICalcularUpsJob>(
                (calcularUpsJob) => 
                    calcularUpsJob.ExecutarAsync(filtro, novoRanque));

            novoRanque.DataFim = DateTimeOffset.Now;
            await dbContext.SaveChangesAsync();
        }
    }

    public class LocalizacaoEscola
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}