using api;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;


namespace app.Controllers
{
    [ApiController]
    [Route("api/escolas")]
    public class EscolaController : ControllerBase
    {
        private readonly IEscolaService escolaService;

        public EscolaController(IEscolaService escolaService)
        {
            this.escolaService = escolaService;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("cadastrarEscolaPlanilha")]
        public async Task<IActionResult> EnviarPlanilha(IFormFile arquivo)
        {

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
                    escolasNovas = await escolaService.CadastrarEscolaViaPlanilhaAsync(memoryStream);
                }

                return Ok(escolasNovas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("obter")]
        public IActionResult ObterEscolas([FromQuery] PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            ListaPaginada<EscolaCorretaModel> listaEscolaPaginada = escolaService.Obter(pesquisaEscolaFiltro);

            return new OkObjectResult(listaEscolaPaginada);
        }

        [HttpDelete("excluir")]
        public IActionResult ExcluirEscola([FromQuery] int id)
        {
            escolaService.ExcluirEscola(id);
            return Ok();

        }

        [HttpPost("cadastrarEscola")]
        public async Task<IActionResult> CadastrarEscolaAsync(CadastroEscolaDTO cadastroEscolaDTO)
        {
            await escolaService.CadastrarEscolaAsync(cadastroEscolaDTO);
            return Ok();
        }

        [HttpPost("removerSituacao")]
        public IActionResult RemoverSituacao([FromQuery] int idEscola)
        {
            escolaService.RemoverSituacaoEscola(idEscola);
            return Ok();
        }

        [HttpPut("alterarDadosEscola")]
        public IActionResult AlterarDadosEscola([FromBody] AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
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
