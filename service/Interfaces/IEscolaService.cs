using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public IEnumerable<Escola> Listar();
        public void Excluir(ExclusaoEscola exclusaoEscola);

        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);

    }

}

  


