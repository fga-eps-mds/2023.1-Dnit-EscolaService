using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Net.Mail;

namespace app.Controllers
{
    [ApiController]
    [Route("api/solicitacaoAcao")]
    public class SolicitacaoAcaoController : ControllerBase
    {
        private readonly ISolicitacaoAcaoService solicitacaoAcaoService;

        public SolicitacaoAcaoController(ISolicitacaoAcaoService solicitacaoAcaoService)
        {
            this.solicitacaoAcaoService = solicitacaoAcaoService;
        }
        [HttpPost]
        public IActionResult EnviarSolicitacaoAcao([FromBody] SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            try
            {
                solicitacaoAcaoService.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);
                return Ok();
            } catch(SmtpException)
            {
                return StatusCode(500, "Falha no envio do email.");
            }
        }
    }
}
