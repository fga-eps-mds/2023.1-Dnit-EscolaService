using AutoMapper;
using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Diagnostics;
using System.Web;

namespace app.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class EscolaController : ControllerBase
    {

        private readonly IEscolaService escolaService;

        public EscolaController(IEscolaService escolaService)
        {
            this.escolaService = escolaService;
        }
        
        [HttpGet("ListarInformacoesEscolas")]
        public IActionResult ListarInformacoesEscolas([FromQuery] int id_escola)
        {
            Escola escola = escolaService.ListarInformacoesEscolas(id_escola);
            return Ok(escola);
        }

        [HttpPost("AdicionarSituacao")]
        public IActionResult AdicionarSituacao([FromBody] AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaService.AdicionarSituacao(atualizarSituacaoDTO);
            return Ok();
        }
    }
}
