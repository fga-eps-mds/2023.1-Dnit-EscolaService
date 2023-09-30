using api.Escolas;
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
                    escolasNovas = await escolaService.CadastrarAsync(memoryStream);
                }

                return Ok(escolasNovas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("obter")]
        public async Task<ListaEscolaPaginada<EscolaCorretaModel>> ObterEscolas([FromQuery] PesquisaEscolaFiltro filtro)
        {
            return await escolaService.ListarPaginadaAsync(filtro);
        }

        [HttpDelete("excluir")]
        public async Task ExcluirEscola([FromQuery] Guid id)
        {
            await escolaService.ExcluirAsync(id);
        }

        [HttpPost("cadastrarEscola")]
        public async Task CadastrarEscolaAsync(CadastroEscolaDTO cadastroEscolaDTO)
        {
            await escolaService.CadastrarAsync(cadastroEscolaDTO);
        }

        [HttpPost("removerSituacao")]
        public async Task RemoverSituacao([FromQuery] Guid idEscola)
        {
            await escolaService.RemoverSituacaoAsync(idEscola);
        }

        [HttpPut("alterarDadosEscola")]
        public async Task AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
            await escolaService.AlterarDadosEscolaAsync(atualizarDadosEscolaDTO);
        }
    }
}
