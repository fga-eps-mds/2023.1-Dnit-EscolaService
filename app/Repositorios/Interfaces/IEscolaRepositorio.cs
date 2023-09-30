using api;
using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEscolaRepositorio
    {
        Task<Escola> ObterPorIdAsync(Guid id, bool incluirEtapas = false, bool incluirMunicipio = false);
        Escola Criar(CadastroEscolaDTO escolaData, Municipio municipio);
        Escola Criar(EscolaModel escola);
        Task<ListaPaginada<Escola>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro);
        Task<Escola?> ObterPorCodigoAsync(int codigo, bool incluirEtapas = false, bool incluirMunicipio = false);
        EscolaEtapaEnsino AdicionarEtapaEnsino(Escola escola, EtapaEnsino etapa);
    }
}
