using Microsoft.EntityFrameworkCore;

using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using service.Interfaces;
using Hangfire;
using api;
using api.Ranques;
using Microsoft.Extensions.Options;

namespace app.Services
{
    public class RanqueService : IRanqueService
    {
        private readonly AppDbContext dbContext;
        private readonly IRanqueRepositorio ranqueRepositorio;
        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly ModelConverter mc;
        private readonly IBackgroundJobClient jobClient;
        private int ExpiracaoMinutos { get; set; }
        private int TamanhoBatelada { get; set; }

        public RanqueService(
            AppDbContext dbContext,
            IRanqueRepositorio ranqueRepositorio,
            IEscolaRepositorio escolaRepositorio,
            ModelConverter mc,
            IOptions<CalcularUpsJobConfig> calcularUpsJobConfig,
            IBackgroundJobClient jobClient
        )
        {
            this.dbContext = dbContext;
            this.ranqueRepositorio = ranqueRepositorio;
            this.escolaRepositorio = escolaRepositorio;
            this.jobClient = jobClient;
            this.mc = mc;
            ExpiracaoMinutos = calcularUpsJobConfig.Value.ExpiracaoMinutos;
            TamanhoBatelada = calcularUpsJobConfig.Value.TamanhoBatelada;
        }

        public async Task CalcularNovoRanqueAsync()
        {
            var totalEscolas = await dbContext.Escolas.CountAsync();
            var filtro = new PesquisaEscolaFiltro { TamanhoPagina = TamanhoBatelada };
            var totalPaginas = (int)Math.Ceiling((double)totalEscolas / TamanhoBatelada);

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
                jobClient.Enqueue<ICalcularUpsJob>((calcularUpsJob) =>
                    calcularUpsJob.ExecutarAsync(filtro, novoRanque.Id, ExpiracaoMinutos));
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

            var items = resultado.Items.Select((i, index) => mc
                .ToModel(i, ((resultado.Pagina - 1) * resultado.ItemsPorPagina) + index + 1))
                .ToList();
            return new ListaPaginada<RanqueEscolaModel>(items, resultado.Pagina, resultado.ItemsPorPagina, resultado.Total);
        }

        // ObterUltimoRanque?
        // O que esse serviço faz é obter o status do **último ranque**, esteja ele em processamento ou não
        public async Task<RanqueEmProcessamentoModel> ObterRanqueEmProcessamento()
        {
            var ultimoRanque = await ranqueRepositorio.ObterRanqueEmProcessamentoAsync();
            // FIXME: O que mandar aqui?
            if (ultimoRanque == null)
                return new RanqueEmProcessamentoModel();

            var ranque = new RanqueEmProcessamentoModel
            {
                Id = ultimoRanque.Id,
                DataFim = ultimoRanque.DataFim,
                DataInicio = ultimoRanque.DataInicio,
                // precisa checar por DataFim?
                EmProgresso = ultimoRanque.BateladasEmProgresso > 0 || ultimoRanque.DataFim == null,
            };

            return ranque;
        }

        public async Task<DetalhesEscolaRanqueModel> ObterDetalhesEscolaRanque(Guid escolaId)
        {
            var escola = await escolaRepositorio.ObterPorIdAsync(escolaId);
            var ranque = await ranqueRepositorio.ObterUltimoRanqueAsync();
            var (escolaRanque, posicao) = await ranqueRepositorio.ObterEscolaRanqueEPosicaoPorIdAsync(escolaId, ranque!.Id);

            // FIXME: Dados mockados. Tem que buscar do banco de dados no futuro.
            FatorModel[] fatores = {
                new() { Nome = "UPS", Peso = 1, Valor = escola.Ups },
            };

            var detalhes = new DetalhesEscolaRanqueModel
            {
                Escola = mc.ToModel(escola),
                RanqueInfo = new RanqueInfo
                {
                    Fatores = fatores,
                    Pontuacao = escolaRanque!.Pontuacao,
                    Posicao = posicao,
                    RanqueId = ranque.Id,
                }
            };

            return detalhes;
        }
    }

    public class LocalizacaoEscola
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}