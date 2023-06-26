using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Collections;
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

        [HttpPost("removerSituacao")]
        public IActionResult RemoverSituacao([FromQuery] int idEscola)
        {
            escolaService.RemoverSituacaoEscola(idEscola);
            return Ok();
        }
    }
}
