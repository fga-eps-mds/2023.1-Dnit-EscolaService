using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Collections;

namespace app.Controllers
{
    [ApiController]
    [Route("escolas")]
    public class EscolaController : Controller
    {
        private readonly IEscolaService service;
        public EscolaController(IEscolaService service)
        {
            this.service = service;
        }
        [HttpGet]
        public IEnumerable<Escola> Listar()
        {
            IEnumerable<Escola> listaEscola = service.Listar();
            return listaEscola;
        }

    }
}
