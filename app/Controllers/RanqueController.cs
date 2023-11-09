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
        private readonly IRanqueService ranqueService;

        public RanqueController(
            ModelConverter modelConverter,
            IMunicipioService municipioService,
            IRanqueService ranqueService
        )
        {
            this.modelConverter = modelConverter;
            this.municipioService = municipioService;
            this.ranqueService = ranqueService;
        }

        [HttpGet("escolas")]
        public ListaPaginada<RanqueEscolaModel> Listar()
        {
            var escolas = new List<RanqueEscolaModel> {
                new() {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 1",
                    Pontuacao = 300
                },
                new() {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 2",
                    Pontuacao = 100
                },
                new() {
                    IdEscola = Guid.NewGuid(),
                    Nome = "Escola 3",
                    Pontuacao = 100
                },
            };
            var lista = new ListaPaginada<RanqueEscolaModel>(escolas, 1, 10, 1);
            return lista;
        }

        [HttpPost("escolas/novo")]
        public async Task NovoRanque()
        {
            await ranqueService.CalcularNovoRanqueAsync();
        }
    }
}
