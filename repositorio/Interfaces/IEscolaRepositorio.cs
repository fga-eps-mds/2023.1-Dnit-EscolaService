using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public void CadastrarEscola(CadastroEscolaDTO  cadastroEscolaDTO);

        public void AdicionarSituacao(int idSituacao, int idEscola);
    }
}
