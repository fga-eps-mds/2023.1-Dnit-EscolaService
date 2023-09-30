using api;
using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEscolaRepositorio
    {
        Task<Escola> ObterPorIdAsync(Guid id);
        Escola Criar(CadastroEscolaDTO escolaData, Municipio municipio);
        Escola Criar(EscolaModel escola);
        Task<ListaPaginada<Escola>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro);
        Task<Escola?> ObterPorCodigoAsync(int codigo);
        EscolaEtapaEnsino AdicionarEtapaEnsino(Escola escola, EtapaEnsino etapa);
    }
}
