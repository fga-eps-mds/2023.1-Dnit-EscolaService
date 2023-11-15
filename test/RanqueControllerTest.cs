using api;
using api.Escolas;
using app.Controllers;
using app.Entidades;
using app.Services;
using test.Fixtures;
using Xunit.Abstractions;

namespace test
{
    public class RanqueControllerTest : AuthTest
    {
        private readonly RanqueController controller;
        private readonly AppDbContext dbContext;
        private readonly PesquisaEscolaFiltro FiltroVazio = new();

        public RanqueControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            controller = fixture.GetService<RanqueController>(testOutputHelper)!;
            AutenticarUsuario(controller);
        }

        [Fact]
        public async void ListarEscolasUltimoRanque_QuandoNaoHouverRanques_RetornaListaVazia()
        {
            var lista = await controller.ListarEscolasUltimoRanque(FiltroVazio);
            Assert.Empty(lista.Items);
        }

        [Fact(Skip = "")]
        public async void CalcularNovoRanqueAsync_QuandoJaTemRanqueEmProcessamento_DeveAgendarNovoProcessamento()
        {
            dbContext.Ranques.Add(new() { BateladasEmProgresso = 1, DataInicio = DateTimeOffset.Now, DataFim = null });
            dbContext.SaveChanges();

            // Adicionar/verificar que tem N jobs na fila
            // TODO: mockar BackgroundJob?
            // https://docs.hangfire.io/en/latest/background-methods/writing-unit-tests.html

            await controller.CalcularRanque();

            // Verificar que tem N+1 jobs na fila
        }

        [Fact(Skip = "Apenas rascunhando")]
        public void CalcularNovoRanqueAsync_QuandoTudoNormal_DeveAtualizarUpsDasEscolasNoBancoDe_dados()
        {
            // Na real eu não sei o que deve acontecee de vdd
        }

        [Fact(Skip = "Apenas rascunhando")]
        public void CalcularRanque_DeveSerAutenticado()
        {
            // Na real eu não sei o que deve retonar de vdd
        }
    }
}
