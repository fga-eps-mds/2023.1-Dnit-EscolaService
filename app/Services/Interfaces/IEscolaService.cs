using api;
using api.Escolas;
using app.Entidades;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        Task CadastrarAsync(CadastroEscolaDTO cadastroEscolaDTO);
        Task<List<string>> CadastrarAsync(MemoryStream planilha);
        Task AtualizarAsync(Escola escola, EscolaModel data, List<EtapaEnsino>? etapas = null);
        Task<ListaEscolaPaginada<EscolaCorretaModel>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro);
        Task ExcluirAsync(Guid id);
        bool SuperaTamanhoMaximo(MemoryStream planilha);
        Task RemoverSituacaoAsync(Guid id);
        Task AlterarDadosEscolaAsync(AtualizarDadosEscolaDTO dados);
    }
}