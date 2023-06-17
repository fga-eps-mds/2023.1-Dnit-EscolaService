using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public IEnumerable<Escola> Listar();
        public void ExcluirEscola(int id);

        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);

    }

}

  


