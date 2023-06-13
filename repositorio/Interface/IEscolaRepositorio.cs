using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public Escola ListarInformacoesEscola(int idEscola);
        public void AdicionarSituacao(int idSituacao, int idEscola);
    }
}
