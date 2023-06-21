using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public ListaPaginada<Escola> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);
    }
}
