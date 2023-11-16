using System.Collections.Generic;
using System.Linq;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.Extensions.Options;
using Moq;
using service.Interfaces;
using test.Fixtures;
using test.Stubs;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class RanqueServiceTest : TestBed<Base>
    {
        private readonly IRanqueService service;
        private readonly AppDbContext db;
        private readonly PesquisaEscolaFiltro FiltroVazio = new();
        private readonly Mock<IBackgroundJobClient> jobClientMock;
        private readonly CalcularUpsJobConfig jobConfig = new() { ExpiracaoMinutos = -1, TamanhoBatelada = 10 };

        public RanqueServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            db = fixture.GetService<AppDbContext>(testOutputHelper)!;
            db.Clear();

            var ranqueRepo = fixture.GetService<IRanqueRepositorio>(testOutputHelper)!;
            var escolaRepo = fixture.GetService<IEscolaRepositorio>(testOutputHelper)!;

            jobClientMock = new Mock<IBackgroundJobClient>();
            service = new RanqueService(
                db,
                ranqueRepo,
                escolaRepo,
                new ModelConverter(),
                Options.Create(jobConfig),
                jobClientMock.Object
            );
        }

        [Fact]
        public async void ListarEscolasUltimoRanque_QuandoNaoHouverRanques_RetornaListaVazia()
        {
            var lista = await service.ListarEscolasUltimoRanqueAsync(FiltroVazio);
            Assert.Empty(lista.Items);
        }

        [Fact]
        public async void ListarEscolasUltimoRanque_TiverUmRanque_RetornaEscolasDoRanque()
        {
            var escolas = db.PopulaEscolas(3);
            GeraRanque(escolas);

            var lista = await service.ListarEscolasUltimoRanqueAsync(FiltroVazio);
            Assert.Equal(escolas.Count, lista.Items.Count);
        }

        [Fact]
        public async void CalcularNovoRanqueAsync_QuandoNumeroDePaginas10_EnqueueDeveSerChamado10vezes()
        {
            db.PopulaEscolas(33);
            var chamadasEsperadas = (int)Math.Ceiling(33.0 / jobConfig.TamanhoBatelada);

            await service.CalcularNovoRanqueAsync();

            // https://docs.hangfire.io/en/latest/background-methods/writing-unit-tests.html
            jobClientMock.Verify(x => x.Create(
                It.Is<Job>(job => job.Method.Name == "ExecutarAsync"),
                It.IsAny<EnqueuedState>()),
                Times.Exactly(chamadasEsperadas));
        }

        [Fact]
        public async void ObterEscolaEmRanqueDetalhes_QuandoEscolaExiste_RetornaPosicaoCorreta()
        {
            var escolas = db.PopulaEscolas(3);
            var (escolaRanques, _) = GeraRanque(escolas);

            var detalhes = await service.ObterDetalhesEscolaRanque(escolas[0].Id);

            Assert.Equal(escolaRanques[0].EscolaId, detalhes.Escola.IdEscola);
        }

        [Fact]
        public async void ObterRanqueEmProcessamento_QuandoNaoTemRanque_RetornaRanqueComEmProgressoFalso()
        {
            // Esse teste tem que melhorar. Deixar mais claro que é Ranque Vazio
            var ranque = await service.ObterRanqueEmProcessamento();
            Assert.False(ranque.EmProgresso);
        }

        [Fact]
        public async void ObterRanqueEmProcessamento_QuandoTemRanque_RetornaRanque()
        {
            var ranqueId = 1;
            var dataFim = DateTimeOffset.Now;
            db.Ranques.Add(new() { BateladasEmProgresso = 1, Id = ranqueId, DataFim = dataFim });
            db.SaveChanges();

            var ranque = await service.ObterRanqueEmProcessamento();

            Assert.True(ranque.EmProgresso);
            Assert.Equal(dataFim, ranque.DataFim);
        }

        [Fact]
        public async void ConcluirEscolaRanqueAsync_QuandoNormal_EscolasSaoPosicionadasCorretamente()
        {
            var escolas = db.PopulaEscolas(5);
            var (_, ranque) = GeraRanque(escolas, definirPosicao: false);

            await service.ConcluirRanqueamentoAsync(ranque);

            var ers = db.EscolaRanques.OrderBy(e => e.Posicao).ToList();
            for (int i = 1; i < ers.Count; i++)
                Assert.True(ers[i - 1].Posicao + 1 == ers[i].Posicao);
        }

        private (List<EscolaRanque>, Ranque) GeraRanque(List<Escola> escolas, bool definirPosicao = true)
        {
            var ranque = new Ranque { Id = Random.Shared.Next(), DataInicio = DateTimeOffset.Now, DataFim = DateTimeOffset.Now, BateladasEmProgresso = 0 };
            db.Ranques.Add(ranque);

            var escolasRanques = new List<EscolaRanque>(escolas.Count);
            for (int i = 0; i < escolas.Count; i++)
                escolasRanques.Add(new()
                {
                    EscolaId = escolas[i].Id,
                    RanqueId = ranque.Id,
                    Pontuacao = i,
                });

            if (definirPosicao)
                for (int i = 0; i < escolas.Count; i++)
                    escolasRanques[i].Posicao = i + 1;

            db.EscolaRanques.AddRange(escolasRanques);
            db.SaveChanges();
            return (escolasRanques, ranque);
        }

        protected new void Dispose()
        {
            db.Clear();
        }
    }
}
