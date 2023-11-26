using api.Escolas;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using System.Net.Mail;

namespace app.Controllers
{
    [ApiController]
    [Route("api/solicitacaoAcao")]
    public class SolicitacaoAcaoController : AppController
    {
        private readonly ISolicitacaoAcaoService solicitacaoAcaoService;

        public SolicitacaoAcaoController(ISolicitacaoAcaoService solicitacaoAcaoService)
        {
            this.solicitacaoAcaoService = solicitacaoAcaoService;
        }

        [HttpPost]
        public async Task<IActionResult> EnviarSolicitacaoAcao([FromBody] SolicitacaoAcaoData solicitacaoAcaoDTO)
        {
            try
            {
                solicitacaoAcaoService.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);
                await solicitacaoAcaoService.Criar(solicitacaoAcaoDTO);
                return Ok();
            }
            catch (SmtpException)
            {
                return StatusCode(500, "Falha no envio do email.");
            }
        }

        [HttpGet("escolas")]
        public async Task<IEnumerable<EscolaInep>> ObterEscolas([FromQuery] int municipio)
        {
            var escolas = await solicitacaoAcaoService.ObterEscolas(municipio);
            return escolas;
        }
    }
}
