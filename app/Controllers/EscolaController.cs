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
        private readonly IEscolaService service;
        public EscolaController(IEscolaService service)
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
