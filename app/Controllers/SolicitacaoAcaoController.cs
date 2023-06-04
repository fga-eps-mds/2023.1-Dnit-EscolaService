using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/solicitacaoacao")]
    public class SolicitacaoAcaoController
    {
        private readonly ISolicitacaoAcaoService solicitacaoAcaoService;

        public SolicitacaoAcaoController(ISolicitacaoAcaoService solicitacaoAcaoService)
        {
            this.solicitacaoAcaoService = solicitacaoAcaoService;
        }
        [HttpPost("SolicitacaoAcao")]
        public void EnviarSolicitacaoAcao([FromBody] SolicitacaoAcaoDTO solicitacaoacaoDTO)
        {
            solicitacaoAcaoService.EnviarSolicitacaoAcao(solicitacaoacaoDTO);

        }
    }
}
