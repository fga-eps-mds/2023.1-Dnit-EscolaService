using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {

        public IEnumerable<Escola> Obter();
        public void ExcluirEscola(int Id);

        public Escola Obter(int idEscola);
        public void AdicionarSituacao(int idSituacao, int idEscola);

    }
}
