using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public IEnumerable<Escola> Listar();
        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);
    }
}
