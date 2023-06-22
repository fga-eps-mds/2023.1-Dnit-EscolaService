using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {

        public void CadastrarEscola(CadastroEscolaDTO  cadastroEscolaDTO);
        public IEnumerable<Escola> Obter();
        public void ExcluirEscola(int Id);
        public Escola Obter(int idEscola);

        public void AdicionarSituacao(int idSituacao, int idEscola);
        public void RemoverSituacaoEscola(int idEscola);
    }
}
