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

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly IDominioRepositorio dominioRepositorio;
        private readonly IConfiguration configuration;

        public DominioController(IDominioRepositorio dominioRepositorio, IConfiguration configuration)
        {
            this.dominioRepositorio = dominioRepositorio;
            this.configuration = configuration;
        }

        [HttpGet("login/teste")]
        public IResult LoginTeste(string username, string password)
        {
            if (username != "joydip" || password != "joydip123")
            {
                return Results.Unauthorized();
            }

            var configuracaoAutenticaco = configuration.GetSection("Autenticacao");

            var issuer = configuracaoAutenticaco["Issuer"];
            var audience = configuracaoAutenticaco["Audience"];
            var key = Encoding.ASCII.GetBytes(configuracaoAutenticaco["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Email, "email@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuracaoAutenticaco["ExpireMinutes"]!)),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);
        }
        [HttpGet("unidadeFederativa")]
        [Authorize]
        public IActionResult ObterListaUF()
        {
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