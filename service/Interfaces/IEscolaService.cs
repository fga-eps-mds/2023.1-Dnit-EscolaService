using System.IO;
using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(Escola escola);
        public void CadastrarEscolaViaPlanilha(MemoryStream planilha);
        public IEnumerable<Escola> Listar();
        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);

    }
}
