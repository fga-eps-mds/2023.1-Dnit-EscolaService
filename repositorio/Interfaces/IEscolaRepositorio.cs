using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public IEnumerable<EscolaCadastrada> ObterEscolas();
        /*
        public EscolaCadastrada ListarEscolasSituacao(string situacao);
        public EscolaCadastrada ListarEscolasMunicipio(string situacao);
        public EscolaCadastrada ListarEscolasUF(string uf);
        public EscolaCadastrada ListarEscolasEtapas(string etapas_ensino);
        public EscolaCadastrada ListarEscolasNome(string nome);
        */
    }
}