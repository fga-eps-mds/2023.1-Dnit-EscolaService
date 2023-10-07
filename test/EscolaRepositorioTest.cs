using api;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using test.Fixtures;
using test.Stub;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{

    public class EscolaRepositorioTest : TestBed<Base>, IDisposable
    {
        IEscolaRepositorio escolaRepositorio;
        AppDbContext dbContext;

        public EscolaRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            dbContext.CaminhoArquivoMunicipios = Path.Join("..", "..", "..", "Stub", "municipios.csv");

            escolaRepositorio = fixture.GetService<IEscolaRepositorio>(testOutputHelper);
        }

        [Fact]
        public async Task ListarPaginadaAsync_QuandoFiltroForemNulos_DeveRetornarListaDeEscolasPaginadas()
        {
            var escolaDb = dbContext.SeedEscolas(5);

            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1;
            filtro.TamanhoPagina = 2;

            var listaPaginada = await escolaRepositorio.ListarPaginadaAsync(filtro);

            Assert.Equal(filtro.Pagina, listaPaginada.Pagina);
            Assert.Equal(filtro.TamanhoPagina, listaPaginada.ItemsPorPagina);
            Assert.Equal(escolaDb.Count, listaPaginada.Total);
            Assert.Equal(escolaDb.OrderBy(e => e.Nome).First().Codigo, listaPaginada.Items.First().Codigo);
        }

        [Fact]
        public async void ListarPaginadaAsync_QuandoFiltroForPassado_DeveRetornarListaDeEscolasFiltradas()
        {
            var escolaDb = dbContext.SeedEscolas(5);

            var escolaPesquisa = escolaDb.First();
            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1;
            filtro.TamanhoPagina = 2;
            filtro.Nome = escolaPesquisa.Nome;
            filtro.IdUf = (int?)escolaPesquisa.Uf;
            filtro.IdSituacao = (int?)escolaPesquisa.Situacao;
            filtro.IdEtapaEnsino = new List<int>() { (int)(escolaPesquisa.EtapasEnsino.FirstOrDefault()?.EtapaEnsino ?? 0 )};
            filtro.IdMunicipio = escolaPesquisa.MunicipioId;

            var listaPaginada = await escolaRepositorio.ListarPaginadaAsync(filtro);

            Assert.Contains(escolaPesquisa, listaPaginada.Items);
        }

        [Fact]
        public async void ListarPaginadaAsync_QuandoFiltroNaoExistir_DeveRetornarListaVazia()
        {
            dbContext.SeedEscolas(5);

            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1000;
            filtro.TamanhoPagina = 20;

            var listaPaginada = await escolaRepositorio.ListarPaginadaAsync(filtro);

            Assert.Empty(listaPaginada.Items);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoExistir_DeveRetornarEscola()
        {
            var escolaDb = dbContext.SeedEscolas(2);
            var escolaEsperada = escolaDb.Last();
            var escola = await escolaRepositorio.ObterPorIdAsync(escolaEsperada.Id);

            Assert.Equal(escolaEsperada, escola);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoIncluir_DeveRetornarEscola()
        {
            var escolaDb = dbContext.SeedEscolas(2);
            var escolaEsperada = escolaDb.Last();
            var escola = await escolaRepositorio.ObterPorIdAsync(escolaEsperada.Id, incluirEtapas: true, incluirMunicipio: true);

            Assert.Equal(escolaEsperada.Id, escola.Id);
            Assert.Equal(escolaEsperada.Municipio, escola.Municipio);
            Assert.Equal(escolaEsperada.EtapasEnsino, escola.EtapasEnsino);
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoNaoExistir_DeveLancarExcecao()
        {
            var escolaDb = dbContext.SeedEscolas(2);
            var excecao = await Assert.ThrowsAsync<ApiException>(() => escolaRepositorio.ObterPorIdAsync(Guid.NewGuid()));

            Assert.Equal(ErrorCodes.EscolaNaoEncontrada, excecao.Error.Code);
        }

        [Fact]
        public async Task ObterPorCodigoAsync_QuandoExistir_DeveRetornarEscola()
        {
            var escolaDb = dbContext.SeedEscolas(2);
            var escolaEsperada = escolaDb.Last();
            var escola = await escolaRepositorio.ObterPorCodigoAsync(escolaEsperada.Codigo);

            Assert.Equal(escolaEsperada, escola);
        }

        [Fact]
        public async Task ObterPorCodigoAsync_QuandoNaoExistir_DeveRetornarNulo()
        {
            var escolaDb = dbContext.SeedEscolas(2);
            var escola = await escolaRepositorio.ObterPorCodigoAsync(-1);

            Assert.Null(escola);
        }

        [Fact]
        public void AdicionarEtapaEnsino_QuandoValida_DeveAdicionar()
        {
            var escolaDb = dbContext.SeedEscolas(1, comEtapas: false).First();

            var etapa = EtapaEnsino.Infantil;
            var etapaDb = escolaRepositorio.AdicionarEtapaEnsino(escolaDb, etapa);

            Assert.Equal(etapa, etapaDb.EtapaEnsino);
            Assert.Equal(escolaDb, etapaDb.Escola);
            Assert.Equal(1, escolaDb.EtapasEnsino?.Count);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.Municipios);
            dbContext.RemoveRange(dbContext.Escolas);
            dbContext.SaveChanges();
        }
    }
}
