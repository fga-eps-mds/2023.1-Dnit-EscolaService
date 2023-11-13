﻿using api;
using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IRanqueRepositorio
    {
        Task<ListaPaginada<EscolaRanque>> ListarEscolasAsync(int ranqueId, PesquisaEscolaFiltro filtro);
        Task<Ranque?> ObterUltimoRanqueAsync();
    }
}