using api;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEscolaRepositorio
    {
        public int? CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public Escola Criar(EscolaModel escola);
        public void ExcluirEscola(int Id);
        public ListaPaginada<EscolaCorretaModel> ObterEscolas(PesquisaEscolaFiltro pesquisaEscolaFiltro);
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
