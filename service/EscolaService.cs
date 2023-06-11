using service.Interfaces;
using repositorio.Interfaces;
using dominio;

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
    }
}
