using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;

namespace service
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        public EscolaService(IEscolaRepositorio escolaRepositorio)
        {
            this.escolaRepositorio = escolaRepositorio;
        }
        public IEnumerable<Escola> Listar()
        {
            return escolaRepositorio.ObterEscolas();
        }
    }    
}