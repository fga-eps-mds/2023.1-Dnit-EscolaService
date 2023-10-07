using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using test.Fixtures;
using test.Stub;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class MunicipioRepositorioTest : TestBed<Base>, IDisposable
    {
        IMunicipioRepositorio municipioRepositorio;
        AppDbContext dbContext;

        public MunicipioRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper);
            dbContext.CaminhoArquivoMunicipios = Path.Join("..", "..", "..", "Stub", "municipios.csv");
            municipioRepositorio = fixture.GetService<IMunicipioRepositorio>(testOutputHelper);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoExistir_DeveRetornarOMunicipio()
        {
            var municipio = dbContext.SeedMunicipios(1).First();
            var resultado = await municipioRepositorio.ObterPorIdAsync(municipio.Id);
            Assert.Equal(municipio.Id, resultado.Id);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoNaoExistir_DeveLancarExcecao()
        {
            var excecao = await Assert
                .ThrowsAsync<ApiException>(() => municipioRepositorio.ObterPorIdAsync(0));
            Assert.Equal(api.ErrorCodes.MunicipioNaoEncontrado, excecao.Error.Code);
        }

        [Fact]
        public async Task ListarAsync_QuandoVazio_DeveRetornarListaVazia()
        {
            var municipios = await municipioRepositorio.ListarAsync(null);
            Assert.Empty(municipios);
        }

        [Fact]
        public async Task ListarAsync_QuandoPreenchido_DeveRetornarListaCompleta()
        {
            var municipiosDb = dbContext.SeedMunicipios(5);
            var municipios = await municipioRepositorio.ListarAsync(null);
            Assert.Equal(municipiosDb?.Count, municipios.Count);
            Assert.True(municipiosDb?.All(mdb => municipios.Exists(m => m.Id == mdb.Id)));
        }

        [Fact]
        public async Task ListarAsync_QuandoFiltroUf_DeveRetornarListaFiltrada()
        {
            var municipiosDb = dbContext.SeedMunicipios(5);
            var filtroUf = municipiosDb.First().Uf;
            var municipios = await municipioRepositorio.ListarAsync(filtroUf);
            Assert.Equal(municipiosDb.Where(m => m.Uf == filtroUf).Count(), municipios.Count);
        }

        [Fact]
        public async Task ListarAsync_QuandoNaoExistirUfPesquisada_DeveRetornarListaVazia()
        {
            var municipiosDb = dbContext.SeedMunicipios(5);
            var filtroUf = api.UF.DF;

            dbContext.RemoveRange(dbContext.Municipios.Where(m => m.Uf == filtroUf));
            dbContext.SaveChanges();

            var municipios = await municipioRepositorio.ListarAsync(filtroUf);
            Assert.Empty(municipios);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.Municipios);
            dbContext.SaveChanges();
        }
    }
}
