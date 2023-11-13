using api;
using api.Ranques;

namespace service.Interfaces
{
    public interface IRanqueService
    {
        Task CalcularNovoRanqueAsync(int tamanhoBatelada);
        Task<ListaPaginada<RanqueEscolaModel>> ListarEscolasUltimoRanqueAsync(api.Escolas.PesquisaEscolaFiltro filtro);
    }
}