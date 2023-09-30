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
        public void ExcluirEscola(int Id);
        public void RemoverSituacaoEscola(int idEscola);
        public bool EscolaJaExiste(int codigoEscola);
        public void AtualizarDadosPlanilha(EscolaModel escola);
        public int? AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO);
        public void CadastrarEtapasDeEnsino(int idEscola, int idEtapaEnsino);
        public void RemoverEtapasDeEnsino(int idEscola);
        Task<Escola?> ObterPorCodigoAsync(int codigo);
        EscolaEtapaEnsino AdicionarEtapaEnsino(Escola escola, EtapaEnsino etapa);
    }
}
