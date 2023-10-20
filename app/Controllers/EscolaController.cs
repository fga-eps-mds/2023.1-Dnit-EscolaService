using app.Services;
using dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.ComponentModel;

namespace app.Controllers
{
    [ApiController]
    [Route("api/escolas")]
    public class EscolaController : ControllerBase
    {
        private readonly IEscolaService escolaService;
        private readonly AuthService authService;

        public EscolaController(IEscolaService escolaService, AuthService authService)
        {
            this.escolaService = escolaService;
            this.authService = authService;
        }

        public enum Permissao
        {
            [Description("Cadastrar Escola")]
            EscolaCadastrar = 1000,

            [Description("Editar Escola")]
            EscolaEditar = 1001,

            [Description("Remover Escola")]
            EscolaRemover = 1002,

            [Description("Visualizar Escola")]
            EscolaVisualizar = 1003,
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
                Permissions = new List<Permissao>() { Permissao.EscolaVisualizar, Permissao.EscolaRemover, Permissao.EscolaCadastrar },
            });
            return Results.Ok(token);
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("cadastrarEscolaPlanilha")]
        public async Task<IActionResult> EnviarPlanilha(IFormFile arquivo)
        {
            authService.Require(User, Permissao.EscolaCadastrar);
            List<string> escolasNovas;

            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return BadRequest("Nenhum arquivo enviado.");

                if (arquivo.ContentType.ToLower() != "text/csv")
                {
                    return BadRequest("O arquivo deve estar no formato CSV.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await arquivo.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (escolaService.SuperaTamanhoMaximo(memoryStream))
                    {
                        return StatusCode(406, "Tamanho máximo de arquivo ultrapassado!");
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    await arquivo.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    escolasNovas = escolaService.CadastrarEscolaViaPlanilha(memoryStream);
                }

                return Ok(escolasNovas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("obter")]
        public IActionResult ObterEscolas([FromQuery] PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            authService.Require(User, Permissao.EscolaVisualizar);

            ListaPaginada<EscolaCorreta> listaEscolaPaginada = escolaService.Obter(pesquisaEscolaFiltro);

            return new OkObjectResult(listaEscolaPaginada);
        }

        [Authorize]
        [HttpDelete("excluir")]
        public IActionResult ExcluirEscola([FromQuery] int id)
        {
            authService.Require(User, Permissao.EscolaRemover);
            escolaService.ExcluirEscola(id);
            return Ok();
        }

        [Authorize]
        [HttpPost("cadastrarEscola")]
        public IActionResult CadastrarEscola([FromBody] CadastroEscolaDTO cadastroEscolaDTO)
        {
            authService.Require(User, Permissao.EscolaCadastrar);
            escolaService.CadastrarEscola(cadastroEscolaDTO);
            return Ok();
        }

        [Authorize]
        [HttpPost("removerSituacao")]
        public IActionResult RemoverSituacao([FromQuery] int idEscola)
        {
            authService.Require(User, Permissao.EscolaRemover);
            escolaService.RemoverSituacaoEscola(idEscola);
            return Ok();
        }

        [Authorize]
        [HttpPut("alterarDadosEscola")]
        public IActionResult AlterarDadosEscola([FromBody] AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
            authService.Require(User, Permissao.EscolaEditar);

            try
            {
                escolaService.AlterarDadosEscola(atualizarDadosEscolaDTO);
                return Ok();
            }
            catch (Npgsql.PostgresException ex)
            {
                if(ex.SqlState == "23503")
                {
                    return Conflict("A chave estrangeira é inválida.");
                }
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

    }
}
