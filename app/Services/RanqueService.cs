using Microsoft.EntityFrameworkCore;

using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using service.Interfaces;
using Hangfire;
using api;
using api.Ranques;

namespace app.Services
{
    public class RanqueService : IRanqueService
    {
        private readonly AppDbContext dbContext;
        private readonly IRanqueRepositorio ranqueRepositorio;
        private readonly ModelConverter mc;

        public RanqueService(AppDbContext dbContext, IRanqueRepositorio ranqueRepositorio, ModelConverter mc)
        {
            this.dbContext = dbContext;
            this.ranqueRepositorio = ranqueRepositorio;
            this.mc = mc;
        }

        public async Task CalcularNovoRanqueAsync(int tamanhoBatelada = 100)
        {
            var totalEscolas = await dbContext.Escolas.CountAsync();
            var filtro = new PesquisaEscolaFiltro { TamanhoPagina = tamanhoBatelada };
            var totalPaginas = (int)Math.Ceiling((double)totalEscolas / tamanhoBatelada);

            var novoRanque = new Ranque
            {
                DataInicio = DateTimeOffset.Now,
                BateladasEmProgresso = totalPaginas,
            };
            dbContext.Ranques.Add(novoRanque);
            await dbContext.SaveChangesAsync();

            for (int pagina = 1; pagina <= totalPaginas; pagina++)
            {
                filtro.Pagina = pagina;
                BackgroundJob.Enqueue<ICalcularUpsJob>((calcularUpsJob) =>
                    calcularUpsJob.ExecutarAsync(filtro, novoRanque.Id));
            }

            // TODO: Calcular outros fatores para a pontuação. 
            // Vai ser feito na US 5.

            await dbContext.SaveChangesAsync();
        }

        public async Task<ListaPaginada<RanqueEscolaModel>> ListarEscolasUltimoRanqueAsync(PesquisaEscolaFiltro filtro)
        {
            var ultimoRanque = await ranqueRepositorio.ObterUltimoRanqueAsync();

            if (ultimoRanque == null)
                return new ListaPaginada<RanqueEscolaModel>(new(), filtro.Pagina, filtro.TamanhoPagina, 0);

            var resultado = await ranqueRepositorio.ListarEscolasAsync(ultimoRanque.Id, filtro);

            var items = resultado.Items.Select((i, index) => mc.ToModel(i, ((resultado.Pagina - 1) * resultado.ItemsPorPagina) + index + 1)).ToList();
            return new ListaPaginada<RanqueEscolaModel>(items, resultado.Pagina, resultado.ItemsPorPagina, resultado.Total);
        }
    }

    public class LocalizacaoEscola
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}