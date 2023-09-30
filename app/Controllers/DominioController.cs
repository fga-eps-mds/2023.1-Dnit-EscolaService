using api;
using api.Municipios;
using app.Repositorios;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly ModelConverter modelConverter;
        private readonly MunicipioRepositorio municipioRepositorio;

        public DominioController(
            ModelConverter modelConverter,
            MunicipioRepositorio municipioRepositorio
        )
        {
            this.modelConverter = modelConverter;
            this.municipioRepositorio = municipioRepositorio;
        }

        [HttpGet("unidadeFederativa")]
        public IEnumerable<UfModel> ObterListaUF()
        {
            return Enum.GetValues<UF>().Select(modelConverter.ToModel).OrderBy(uf => uf.Sigla);
        }

        [HttpGet("etapasDeEnsino")]
        public IEnumerable<EtapasdeEnsinoModel> ObterListaEtapasdeEnsino()
        {
            return Enum.GetValues<EtapaEnsino>().Select(modelConverter.ToModel).OrderBy(e => e.Descricao);
        }

        [HttpGet("municipio")]
        public async Task<IEnumerable<MunicipioModel>> ObterListaMunicipio([FromQuery] int? idUf)
        {
            var municipios = await municipioRepositorio.ListarAsync((UF?)idUf);
            return municipios.Select(modelConverter.ToModel);
        }

        [HttpGet("situacao")]
        public IEnumerable<SituacaoModel> ObterListaSituacao()
        {
            return Enum.GetValues<Situacao>().Select(modelConverter.ToModel).OrderBy(s => s.Descricao);
        }
    }
}
