using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using dominio;

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

        [HttpPost("cadastroEscola")]
        public IActionResult CadastrarEscola(Escola escola)
        {
            escolaService.CadastrarEscola(escola);
            return Ok();
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

        [HttpGet("listarEscolas")]
        public IEnumerable<Escola> Listar()
        {
            IEnumerable<Escola> escolas = escolaService.Listar();
            return escolas;
        }

        [HttpGet("listarInformacoesEscola")]
        public IActionResult ListarInformacoesEscola([FromQuery] int idEscola)
        {
            Escola escola = escolaService.Listar(idEscola);
            return Ok(escola);
        }

        [HttpPost("adicionarSituacao")]
        public IActionResult AdicionarSituacao([FromBody] AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaService.AdicionarSituacao(atualizarSituacaoDTO);
            return Ok();
        }
    }
}
