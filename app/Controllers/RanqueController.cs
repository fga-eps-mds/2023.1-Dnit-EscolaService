using api;
using api.Municipios;
using api.Ranques;
using app.Entidades;
using app.Repositorios;
using app.Repositorios.Interfaces;
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
        
        public ListaPaginada<RanqueEscolaModel> Listar(){
            
            return new ListaPaginada<RanqueEscolaModel>(new List<RanqueEscolaModel>{
                new RanqueEscolaModel{
                    IdEscola = Guid.NewGuid(),
                    Nome = "teste",
                    Pontuacao = 0
                }
            },
            1,
            10,
            1
            );
        }
    }
}
