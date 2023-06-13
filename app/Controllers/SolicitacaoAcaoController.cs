using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;

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
        public void EnviarSolicitacaoAcao([FromBody] SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            solicitacaoAcaoService.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);

        }
    }
}
