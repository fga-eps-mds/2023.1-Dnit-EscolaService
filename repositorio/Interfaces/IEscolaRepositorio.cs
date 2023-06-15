using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public Escola Obter(int idEscola);
        public IEnumerable<Escola> Obter();
        public void AdicionarSituacao(int idSituacao, int idEscola);
    }
}
