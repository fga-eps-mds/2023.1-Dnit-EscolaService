using dominio;
using repositorio;
using Microsoft.AspNetCore.Mvc;
using repositorio;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using dominio.Dominio;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly IDominioRepositorio dominioRepositorio;

        public DominioController(IDominioRepositorio dominioRepositorio)
        {
            this.dominioRepositorio = dominioRepositorio;
        }

        [HttpGet("unidadeFederativa")]
        public IActionResult ObterListaUF()
        {
            IEnumerable<UnidadeFederativa> listaUnidadeFederativa = dominioRepositorio.ObterUnidadeFederativa();

            return new OkObjectResult(listaUnidadeFederativa);
        }

        [HttpGet("etapasDeEnsino")]
        public IActionResult ObterListaEtapasdeEnsino()
        {
            IEnumerable<EtapasdeEnsino> listaEtapasdeEnsino = dominioRepositorio.ObterEtapasdeEnsino();

            return new OkObjectResult(listaEtapasdeEnsino);
        }

        [HttpGet("municipio")]
        public IActionResult ObterListaMunicipio([FromQuery] int? idUf)
        {
            IEnumerable<Municipio> listaMunicipio = dominioRepositorio.ObterMunicipio(idUf);

            return new OkObjectResult(listaMunicipio);
        }

        [HttpGet("situacao")]
        public IActionResult ObterListaSituacao()
        {
            IEnumerable<Situacao> listaSituacao = dominioRepositorio.ObterSituacao();

            return new OkObjectResult(listaSituacao);
        }

    }
}