using api;
using api.Escolas;
using api.Ranques;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/ranque")]
    public class RanqueController : AppController
    {
        private readonly IRanqueService ranqueService;
        private readonly AuthService authService;

        public RanqueController(
            IRanqueService ranqueService,
            AuthService authService
        )
        {
            this.ranqueService = ranqueService;
            this.authService = authService;
        }

        [HttpGet("escolas")]
        [Authorize]
        public async Task<ListaPaginada<RanqueEscolaModel>> ListarEscolasUltimoRanque([FromQuery] PesquisaEscolaFiltro filtro)
        {
            authService.Require(Usuario, Permissao.UpsVisualizar);
            return await ranqueService.ListarEscolasUltimoRanqueAsync(filtro);
        }

        [HttpPost("escolas/novo")]
        public async Task NovoRanque()
        {
            int tamanhoBatelada = 100;
            await ranqueService.CalcularNovoRanqueAsync(tamanhoBatelada);
        }
    }
}
