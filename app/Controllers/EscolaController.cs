using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Collections;

namespace app.Controllers
{
    [ApiController]
    [Route("escolas")]
    public class EscolaController : Controller
    {
        private readonly EscolaService service;
        public EscolaController(EscolaService service)
        {
            this.service = service;
        }
        [HttpGet]
        public IEnumerable<EscolaCadastrada> Ler()
        {
            IEnumerable<EscolaCadastrada> listaEscolasCadastradas = service.Listar();
            return listaEscolasCadastradas;
        }

        [HttpDelete ("excluir")]
        public IActionResult Excluir(ExclusaoEscola exclusaoEscola)
        {
            service.Excluir(exclusaoEscola);
            return Ok();

    }

    }
}
