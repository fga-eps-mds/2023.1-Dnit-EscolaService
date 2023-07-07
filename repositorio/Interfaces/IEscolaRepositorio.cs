using dominio;
using System.Collections.Generic;
namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public int? CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public void ExcluirEscola(int Id);
        public ListaPaginada<EscolaCorreta> ObterEscolas(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void RemoverSituacaoEscola(int idEscola);
        public void CadastrarEscola(Escola escola);
        public bool EscolaJaExiste(int codigoEscola);
        public void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO);
        public void CadastrarEtapasDeEnsino(int idEscola, int idEtapaEnsino);
        public void RemoverEtapasDeEnsino(int idEscola);
    }
}
