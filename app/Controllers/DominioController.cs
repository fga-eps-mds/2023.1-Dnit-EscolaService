using dominio;
using repositorio;
using Microsoft.AspNetCore.Mvc;
using repositorio;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using dominio.Dominio;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using app.Services;
using System.ComponentModel;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly IDominioRepositorio dominioRepositorio;
        private readonly IConfiguration configuration;
        private readonly AuthService authService;

        public DominioController(IDominioRepositorio dominioRepositorio, IConfiguration configuration, AuthService authService)
        {
            this.dominioRepositorio = dominioRepositorio;
            this.configuration = configuration;
            this.authService = authService;
        }

        [HttpGet("login/teste")]
        public IResult LoginTeste(string username, string password)
        {
            if (username != "joydip" || password != "joydip123")
            {
                return Results.Unauthorized();
            }
            var (token, expiracao) = authService.GenerateToken(new auth.AuthUserModel<Permissao>
            {
                Id = 12,
                Name = "Cassio",
                Permissions = new List<Permissao>() { Permissao.VisualizarUnidadeFederativa},
            }) ;
            return Results.Ok(token);
        }
        public enum Permissao {
            [Description("Visualizar Unidades Federativas")]
            VisualizarUnidadeFederativa = 5000,
        }

        [HttpGet("unidadeFederativa")]
        [Authorize]
        public IActionResult ObterListaUF()
        {
            authService.Require(User, Permissao.VisualizarUnidadeFederativa);
            IEnumerable<UnidadeFederativa> listaUnidadeFederativa = dominioRepositorio.ObterUnidadeFederativa();
            return new OkObjectResult(listaUnidadeFederativa);
        }

        [HttpGet("etapasDeEnsino")]
        public IActionResult ObterListaEtapasdeEnsino()
        {
            IEnumerable<EtapasdeEnsino> listaEtapasdeEnsino = dominioRepositorio.ObterEtapasdeEnsino();

            return new OkObjectResult(listaEtapasdeEnsino);
        }

        [HttpGet("municipio")]
        public IActionResult ObterListaMunicipio([FromQuery] int? idUf)
        {
            IEnumerable<Municipio> listaMunicipio = dominioRepositorio.ObterMunicipio(idUf);

            return new OkObjectResult(listaMunicipio);
        }

        [HttpGet("situacao")]
        public IActionResult ObterListaSituacao()
        {
            IEnumerable<Situacao> listaSituacao = dominioRepositorio.ObterSituacao();

            return new OkObjectResult(listaSituacao);
        }

    }
}