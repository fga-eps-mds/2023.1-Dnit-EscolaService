using app.Services;
using api.Escolas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> EnviarPlanilhaAsync(IFormFile arquivo)
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
                    escolasNovas = await escolaService.CadastrarAsync(memoryStream);
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
        public async Task<ListaEscolaPaginada<EscolaCorretaModel>> ObterEscolasAsync([FromQuery] PesquisaEscolaFiltro filtro)
        {
            authService.Require(User, Permissao.EscolaVisualizar);

            return await escolaService.ListarPaginadaAsync(filtro);
        }

        [Authorize]
        [HttpDelete("excluir")]
        public async Task ExcluirEscolaAsync([FromQuery] Guid id)
        {
            authService.Require(User, Permissao.EscolaRemover);
            await escolaService.ExcluirAsync(id);
        }

        [Authorize]
        [HttpPost("cadastrarEscola")]
        public async Task CadastrarEscolaAsync(CadastroEscolaData cadastroEscolaDTO)
        {
            authService.Require(User, Permissao.EscolaCadastrar);
            await escolaService.CadastrarAsync(cadastroEscolaDTO);
        }

        [Authorize]
        [HttpPost("removerSituacao")]
        public async Task RemoverSituacaoAsync([FromQuery] Guid idEscola)
        {
            authService.Require(User, Permissao.EscolaRemover);
            await escolaService.RemoverSituacaoAsync(idEscola);
        }

        [Authorize]
        [HttpPut("alterarDadosEscola")]
        public async Task AlterarDadosEscolaAsync(AtualizarDadosEscolaData atualizarDadosEscolaDTO)
        {
            authService.Require(User, Permissao.EscolaEditar);

            await escolaService.AlterarDadosEscolaAsync(atualizarDadosEscolaDTO);
        }
    }
}
