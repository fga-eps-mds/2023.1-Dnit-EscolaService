using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using dominio;

namespace app.Controllers
{
    [ApiController]
    [Route("api/controller")]

    public class EscolaController : ControllerBase
    {

        private readonly IEscolaService escolaService;

        public EscolaController(IEscolaService escolaService)
        {
            this.escolaService = escolaService;
        }

        [HttpPost("cadastroEscola")]
        public IActionResult CadastrarEscola(Escola escola)
        {
            escolaService.CadastrarEscola(escola);
            return Ok();
        }
    }
}