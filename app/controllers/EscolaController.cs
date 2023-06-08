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
        public IActionResult ListarInformacoesEscolas([FromBody] Escola escola)
        {
            escolaService.ListarInformacoesEscolas(escola);
            return Ok();
        }

        [HttpPost("AdicionarSituacao")]
        public IActionResult AdicionarSituacao([FromBody]Escola escola)
        {
            escolaService.AdicionarSituacao(escola);
            return Ok();
        }
    }
}
