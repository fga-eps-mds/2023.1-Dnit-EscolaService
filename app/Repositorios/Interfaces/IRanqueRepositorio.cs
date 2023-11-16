﻿using api;
using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IRanqueRepositorio
    {
        Task<ListaPaginada<EscolaRanque>> ListarEscolasAsync(int ranqueId, PesquisaEscolaFiltro filtro);
        Task<List<EscolaRanque>> ListarEscolasAsync(int ranqueId);
        Task<Ranque?> ObterUltimoRanqueAsync();
        Task<Ranque?> ObterRanqueEmProcessamentoAsync();
        Task<Ranque?> ObterPorIdAsync(int id);
        Task<(EscolaRanque?, int)> ObterEscolaRanqueEPosicaoPorIdAsync(Guid escolaId, int ranqueId);
    }
}