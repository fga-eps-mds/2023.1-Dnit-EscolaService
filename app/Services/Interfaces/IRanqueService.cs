using api;
using api.Escolas;
using api.Ranques;

namespace service.Interfaces
{
    public interface IRanqueService
    {
        Task CalcularNovoRanqueAsync();
        Task<ListaPaginada<RanqueEscolaModel>> ListarEscolasUltimoRanqueAsync(PesquisaEscolaFiltro filtro);
        Task<RanqueEmProcessamentoModel> ObterRanqueEmProcessamento();
        Task<DetalhesEscolaRanqueModel> ObterDetalhesEscolaRanque(Guid escolaId);
    }
}