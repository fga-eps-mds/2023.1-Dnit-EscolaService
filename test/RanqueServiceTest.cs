using System.Collections.Generic;
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
            var ranqueId = 1;
            db.Ranques.Add(new() { DataFim = DateTimeOffset.Now, DataInicio = DateTimeOffset.Now, Id = ranqueId });
            var escolaRanques = GeraEscolaRanques(escolas, ranqueId);
            db.EscolaRanques.AddRange(escolaRanques);
            db.SaveChanges();

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

        [Fact(Skip = "Apenas rascunhando")]
        public void RequisicaoHttpParaUpsServiceDeveSerAutenticado() { }

        [Fact(Skip = "Apenas rascunhando")]
        public void ObterEscolaEmRanqueDetalhes_RetornaPosicaoCorreta()
        {
            // endpoint do olhozinho. O que deve retornar ?
        }

        [Fact(Skip = "O que fazer?")]
        public void CalcularRanque_QuandoHouverErro_TentaDeNovo_AgendaOutraTentativa_SoRetornaErro()
        {
            // Na real eu não sei o que deve retonar de vdd
        }

        private static List<EscolaRanque> GeraEscolaRanques(List<Escola> escolas, int ranqueId)
        {
            var escolasRanques = new List<EscolaRanque>(escolas.Count);
            for (int i = 0; i < escolas.Count; i++)
                escolasRanques.Add(new()
                {
                    EscolaId = escolas[i].Id,
                    RanqueId = ranqueId,
                    Pontuacao = Random.Shared.Next() % 100
                });
            return escolasRanques;
        }

        protected new void Dispose()
        {
            db.Clear();
        }
    }
}
