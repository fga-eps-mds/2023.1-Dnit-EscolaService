using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;

namespace app.Controllers
{
    public class SolicitacaoAcaoController
    {
        private readonly ISolicitacaoAcaoService solicitacaoAcaoService;

        public SolicitacaoAcaoController(ISolicitacaoAcaoService solicitacaoAcaoService)
        {
            this.solicitacaoAcaoService = solicitacaoAcaoService;
        }
        [HttpPost("SolicitacaoAcao")]
        public IActionResult CadastrarSolicitacaoAcao([FromBody] SolicitacaoAcaoDTO solicitacaoacaoDTO)
        {
            solicitacaoAcaoService.CadastrarSolicitacaoAcao(solicitacaoacaoDTO);

            return Ok();
        }
    }
}
