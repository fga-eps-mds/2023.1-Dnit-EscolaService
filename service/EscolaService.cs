using service.Interfaces;
using repositorio.Interfaces;
using dominio;
using System.IO;

namespace service
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;

        public EscolaService(IEscolaRepositorio escolaRepositorio)
        {
            this.escolaRepositorio = escolaRepositorio;
        }

        public void CadastrarEscola(Escola escola)
        {
            escolaRepositorio.CadastrarEscola(escola);
        }

        public void CadastrarEscolaViaPlanilha(MemoryStream planilha)
        {
            /// arquivo vocês precisam ler o memory stream da planilha e enviar
            /// cada linha pro repositório
            throw new System.NotImplementedException();
        }
    }
}
