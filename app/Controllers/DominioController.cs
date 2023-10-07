﻿using api;
using api.Municipios;
using app.Repositorios;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly ModelConverter modelConverter;
        private readonly IMunicipioService municipioService;

        public DominioController(
            ModelConverter modelConverter,
            IMunicipioService municipioService
        )
        {
            this.modelConverter = modelConverter;
            this.municipioService = municipioService;
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
            return await municipioService.ListarAsync((UF?)idUf);
        }

        [HttpGet("situacao")]
        public IEnumerable<SituacaoModel> ObterListaSituacao()
        {
            return Enum.GetValues<Situacao>().Select(modelConverter.ToModel).OrderBy(s => s.Descricao);
        }
    }
}
