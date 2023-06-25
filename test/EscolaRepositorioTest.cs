
using Xunit;
using repositorio;
using repositorio.Interfaces;
using dominio;
using Microsoft.Data.Sqlite;
using System.Data;
using repositorio.Contexto;
using Dapper;
using System;

namespace test
{
   
    public class EscolaRepositorioTest : IDisposable
    {
        IEscolaRepositorio repositorio;
        SqliteConnection connection;
        public EscolaRepositorioTest()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            repositorio = new EscolaRepositorio(contexto => new Contexto(connection));
        }

        [Fact]
        public void ObterEscolas_QuandoFiltroForemNulos_DeveRetornarListaDeEscolasPaginadas()
        {
            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1;
            filtro.TamanhoPagina = 2;

            var listaPaginada = repositorio.ObterEscolas(filtro);

            Assert.Equal(filtro.Pagina, listaPaginada.Pagina);
            Assert.Equal(filtro.TamanhoPagina, listaPaginada.EscolasPorPagina);
            Assert.Equal(3, listaPaginada.TotalEscolas);
            Assert.Equal(2, listaPaginada.TotalPaginas);
            Assert.Equal("CEM02", listaPaginada.Escolas[0].NomeEscola);
            Assert.Equal("CEM03", listaPaginada.Escolas[1].NomeEscola);
        }

        [Fact]
        public void ObterEscolas_QuandoFiltroForPassado_DeveRetornarListaDeEscolasFiltradas()
        {
            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1;
            filtro.TamanhoPagina = 2;
            filtro.Nome = "CEM";
            filtro.IdUf = 1;
            filtro.IdSituacao = 1;
            filtro.IdEtapaEnsino = 1;
            filtro.IdMunicipio = 1;

            var listaPaginada = repositorio.ObterEscolas(filtro);

            Assert.Equal(filtro.Pagina, listaPaginada.Pagina);
            Assert.Equal(filtro.TamanhoPagina, listaPaginada.EscolasPorPagina);
            Assert.Equal(2, listaPaginada.TotalEscolas);
            Assert.Equal(1, listaPaginada.TotalPaginas);
            Assert.Equal("CEM02", listaPaginada.Escolas[0].NomeEscola);
            Assert.Equal("CEM04", listaPaginada.Escolas[1].NomeEscola);
        }
        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
