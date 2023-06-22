using dominio;
using dominio.Dominio;
using repositorio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IDominioRepositorio
    {
        public IEnumerable<UnidadeFederativa> ObterUnidadeFederativa();
        public IEnumerable<EtapasdeEnsino> ObterEtapasdeEnsino();
        public IEnumerable<Municipio> ObterMunicipio();
        public IEnumerable<Situacao> ObterSituacao();

    }
}