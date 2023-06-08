using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public IEnumerable<EscolaCadastrada> Listar();
        public void Excluir(ExclusaoEscola exclusaoEscola);
    }
   
}