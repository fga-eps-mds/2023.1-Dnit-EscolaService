using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public IEnumerable<Escola> Listar();
    }
}