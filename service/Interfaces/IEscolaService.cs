using dominio;
using System.IO;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(Escola escola);
        public void CadastrarEscolaViaPlanilha(MemoryStream planilha);
    }
}
