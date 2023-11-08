using api;
using api.Ranques;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/ranque")]
    public class RanqueController : AppController
    {
        private readonly ModelConverter modelConverter;
        private readonly IMunicipioService municipioService;

        public RanqueController(
            ModelConverter modelConverter,
            IMunicipioService municipioService
        )
        {
            this.modelConverter = modelConverter;
            this.municipioService = municipioService;
        }

        [HttpGet("escolas")]
        public ListaPaginada<RanqueEscolaModel> Listar()
        {
            var escolas = new List<RanqueEscolaModel>{
                new RanqueEscolaModel {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 1",
                    Pontuacao = 300
                },
                new RanqueEscolaModel {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 2",
                    Pontuacao = 100
                },
                new RanqueEscolaModel {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 3",
                    Pontuacao = 100
                },
            };
            var lista = new ListaPaginada<RanqueEscolaModel>(escolas, 1, 10, 1);
            return lista;
        }
    }
}
