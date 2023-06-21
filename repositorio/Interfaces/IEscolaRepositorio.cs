using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public Escola Obter(int idEscola);
        public ListaPaginada<Escola> ObterEcolas(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void AdicionarSituacao(int idSituacao, int idEscola);
    }
}
