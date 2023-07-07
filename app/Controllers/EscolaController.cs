using dominio;
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

            List<int> escolasDuplicadas;

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
                    escolasDuplicadas = escolaService.CadastrarEscolaViaPlanilha(memoryStream);
                }

                return Ok(escolasDuplicadas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
   
        [HttpGet("obter")]
        public IActionResult ObterEscolas([FromQuery] PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            ListaPaginada<Escola> listaEscolaPaginada = escolaService.Obter(pesquisaEscolaFiltro);

            return new OkObjectResult(listaEscolaPaginada);
        }
        [HttpDelete("excluir")]
        public IActionResult ExcluirEscola([FromQuery] int id)
        {
            escolaService.ExcluirEscola(id);
            return Ok();

        }

        [HttpPost("cadastrarEscola")]
        public IActionResult CadastrarEscola([FromBody] CadastroEscolaDTO cadastroEscolaDTO)
        {
            escolaService.CadastrarEscola(cadastroEscolaDTO);
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
            escolaService.AlterarDadosEscola(atualizarDadosEscolaDTO);
            return Ok();
        }

    }
}
