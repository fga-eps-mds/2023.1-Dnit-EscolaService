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
        
        [HttpGet("ListarEscolas")]
        public IActionResult ListarEscola([FromBody] Escola escola)
        {
            escolaService.ListaEscolas(escola);
            return Ok();
        }
    }
}
