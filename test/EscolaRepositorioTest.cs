
using Xunit;
using repositorio;
using repositorio.Interfaces;
using dominio;
using Microsoft.Data.Sqlite;
using System.Data;
using repositorio.Contexto;
using Dapper;
using System;
using System.Runtime.ConstrainedExecution;
using dominio.Dominio;
using test.Stub;

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

        [Fact]

        public void CadastrarEscola_QuandoAEscolaForPassada_DeveCadastrarNoBanco()
        {
            EscolaStub escolaStub = new EscolaStub();
            var escolaEsperada = escolaStub.ObterCadastroEscolaDTO();
            int? idEscola = repositorio.CadastrarEscola(escolaEsperada);
            var escolaObtida = repositorio.Obter(idEscola.Value);
            Assert.Equal(escolaEsperada.NomeEscola, escolaObtida.NomeEscola);
            Assert.Equal(escolaEsperada.CodigoEscola, escolaObtida.CodigoEscola);
            Assert.Equal(escolaEsperada.Endereco, escolaObtida.Endereco);
            Assert.Equal(escolaEsperada.Latitude, escolaObtida.Latitude);
            Assert.Equal(escolaEsperada.Longitude, escolaObtida.Longitude);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
