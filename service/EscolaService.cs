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
        public IEnumerable<EscolaCadastrada> Listar()
        {
            return escolaRepositorio.ObterEscolas();
        }
        public void Excluir(ExclusaoEscola exclusaoEscola)
        {
            escolaRepositorio.Excluir(exclusaoEscola.Id);
        }
    }    
}