using dominio;
using System.Collections.Generic;
namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public int? CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public void ExcluirEscola(int Id);
        public Escola Obter(int idEscola);
        public ListaPaginada<Escola> ObterEscolas(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void AdicionarSituacao(int idSituacao, int idEscola);
        public void RemoverSituacaoEscola(int idEscola);
        public void CadastrarEscola(Escola escola);
        public bool EscolaJaExiste(int codigoEscola);
        public void AlterarTelefone(int idEscola, string telefone);
        public void AlterarLatitude(int idEscola, string latitude);
        public void AlterarLongitude(int idEscola, string longitude);
        public void AlterarNumeroDeAlunos(int idEscola, int numeroDeAlunos);
    }
}
