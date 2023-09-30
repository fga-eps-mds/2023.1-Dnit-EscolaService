﻿using dominio;
using Microsoft.Data.Sqlite;
using test.Stub;
using Dapper;
using repositorio.Interfaces;

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

            repositorio = new repositorio.EscolaRepositorio(contexto => new Contexto(connection));
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
        public void ExcluirEscola_QuandoIdForPassado_DeveExcluirEscolaCorrespondente()
        {
            EscolaStub escolaStub = new EscolaStub();
            var escola = escolaStub.ObterCadastroEscolaDTO();

            int? idEscolaCadastrada = repositorio.CadastrarEscola(escola);
            repositorio.ExcluirEscola(idEscolaCadastrada!.Value);

            string sql = $"SELECT id_escola FROM public.escola WHERE id_escola = {idEscolaCadastrada.Value}";
            int? idEscolaObtida = connection.ExecuteScalar<int?>(sql);

            Assert.Null(idEscolaObtida);
        }

        [Fact]
        public void AlterarDadosEscola_QuandoDadosDaEscolaForemAlterados_DeveRetornarVerdadeiro()
        {
            EscolaStub escolaStub = new EscolaStub();
            var cadastroEscolaDTO = escolaStub.ObterCadastroEscolaDTO();
            var atualizarDadosEscolaDTO = escolaStub.ObterAtualizarDadosEscolaDTO();

            int? idEscolaCadastrada = repositorio.CadastrarEscola(cadastroEscolaDTO);
            atualizarDadosEscolaDTO.IdEscola = idEscolaCadastrada!.Value;
            var linhasAfetadas = repositorio.AlterarDadosEscola(atualizarDadosEscolaDTO);

            int linhasEsperadas = 1;
            Assert.Equal(linhasEsperadas, linhasAfetadas);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
