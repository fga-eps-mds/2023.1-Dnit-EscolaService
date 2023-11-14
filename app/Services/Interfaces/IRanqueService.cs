using api;
using api.Ranques;

namespace service.Interfaces
{
    public interface IRanqueService
    {
        Task CalcularNovoRanqueAsync();
        Task<ListaPaginada<RanqueEscolaModel>> ListarEscolasUltimoRanqueAsync(api.Escolas.PesquisaEscolaFiltro filtro);
        Task<RanqueEmProcessamentoModel> ObterRanqueEmProcessamento();
    }
}