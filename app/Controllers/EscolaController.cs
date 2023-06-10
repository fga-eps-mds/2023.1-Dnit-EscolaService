using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

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
        public IActionResult CadastrarEscola(IFormFile escolas)
        {

        }
    }
}