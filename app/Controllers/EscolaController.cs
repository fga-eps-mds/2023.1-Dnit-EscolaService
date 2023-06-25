using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Collections;

namespace app.Controllers
{
    [ApiController]
    [Route("api/escolas")]
    public class EscolaController : ControllerBase
    {
        private readonly IEscolaService escolaService;
        public EscolaController(IEscolaService service)
        {
            this.escolaService = service;
        }
        [HttpGet]
        public IEnumerable<EscolaCadastrada> Ler()
        {
            IEnumerable<EscolaCadastrada> listaEscolasCadastradas = escolaService.Listar();
            return listaEscolasCadastradas;
        }

        [HttpDelete ("excluir")]
        public IActionResult ExcluirEscola([FromQuery] int id)
        {
            escolaService.ExcluirEscola(id);
            return Ok();

        }

    }
}
