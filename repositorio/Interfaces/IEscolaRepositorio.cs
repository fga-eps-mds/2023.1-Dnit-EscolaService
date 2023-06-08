using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public IEnumerable<EscolaCadastrada> ObterEscolas();
    }
}