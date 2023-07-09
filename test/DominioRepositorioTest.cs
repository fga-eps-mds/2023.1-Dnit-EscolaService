using repositorio;
using repositorio.Interfaces;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Linq;

namespace test
{
    public class DominioRepositorioTest : IDisposable
    {
        IDominioRepositorio repositorio;
        SqliteConnection connection;
        public DominioRepositorioTest()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            repositorio = new DominioRepositorio(contexto => new Contexto(connection));
        }

        [Fact]
        public void ObterUnidadeFederativa_QuandoHouverUFsCadastradas_DeveRetornarListaDeUFs()
        {

            var dominios = repositorio.ObterUnidadeFederativa();

            Assert.Equal("Distrito Federal", dominios.ElementAt(0).Nome);
            Assert.Equal("Goiás", dominios.ElementAt(1).Nome);
            Assert.Equal(2, dominios.Count());
        }
        [Fact]
        public void ObterUnidadeFederativa_QuandoNaoHouverUFsCadastradas_DeveRetornarListaVazia()
        {
            string sql = "DELETE FROM public.unidade_federativa";
            connection.Execute(sql);
            var dominios = repositorio.ObterUnidadeFederativa();

            Assert.Empty(dominios);
        }

        [Fact]
        public void ObterEtapasdeEnsino_QuandoNaoHouverEtapasCadastradas_DeveRetornarListaVazia()
        {
            string sql = "DELETE FROM public.etapas_de_ensino";
            connection.Execute(sql);
            var etapas = repositorio.ObterEtapasdeEnsino();

            Assert.Empty(etapas);
        }

        [Fact]
        public void ObterEtapasdeEnsino_QuandoHouverEtapasCadastradas_DeveRetornarListaDeEtapas()
        {

            var etapas = repositorio.ObterEtapasdeEnsino();

            Assert.Equal("Educação Infantil", etapas.ElementAt(0).Descricao);
            Assert.Equal("Ensino Fundamental", etapas.ElementAt(1).Descricao);
            Assert.Equal(2, etapas.Count());
        }

        [Fact]
        public void ObterSituacao_QuandoNaoHouverSituacoesCadastradas_DeveRetornarListaVazia()
        {
            string sql = "DELETE FROM public.situacao";
            connection.Execute(sql);
            var etapas = repositorio.ObterSituacao();

            Assert.Empty(etapas);
        }

        [Fact]
        public void ObterSituacao_QuandoHouverSituacoesCadastradas_DeveRetornarListaDeSituacoes()
        {

            var etapas = repositorio.ObterSituacao();

            Assert.Equal("A", etapas.ElementAt(0).Descricao);
            Assert.Equal("B", etapas.ElementAt(1).Descricao);
            Assert.Equal(2, etapas.Count());
        }

        [Fact]
        public void ObterMunicipio_QuandoNaoHouverMunicipiosCadastradas_DeveRetornarListaVazia()
        {
            string sql = "DELETE FROM public.municipio";
            connection.Execute(sql);
            var municipios = repositorio.ObterMunicipio(null);

            Assert.Empty(municipios);
        }

        [Fact]
        public void ObterMunicipio_QuandoFiltroForPassado_DeveRetornarListaDeMunicipiosFiltrados()
        {
            int idUF = 1;

            var municipios = repositorio.ObterMunicipio(idUF);

            Assert.Equal("Brasília", municipios.ElementAt(0).Nome);
            Assert.Single(municipios);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
