using app.Controllers;
using dominio;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using test.Stub;

namespace test
{
    public class EscolaControllerTest
    {
        [Fact]
        public void ObterEscolas_QuandoMetodoForChamado_DeveRetornarListaDeEscolas()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            var filtro = new PesquisaEscolaFiltro();
            filtro.Pagina = 1;
            filtro.TamanhoPagina = 2;
            var result = controller.ObterEscolas(filtro);

            escolaServiceMock.Verify(service => service.Obter(filtro), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Excluir_QuandoIdEscolaForPassado_DeveExcluirEscola()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            int idEscola = 59;
            var result = controller.ExcluirEscola(idEscola);

            escolaServiceMock.Verify(service => service.ExcluirEscola(idEscola), Times.Once);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void ListarInformacoesEscola_QuandoEscolaForEncontrada_DeveRetornarEscola()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            int idEscola = 100;
            var result = controller.ListarInformacoesEscola(idEscola);

            escolaServiceMock.Verify(service => service.Listar(idEscola), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void ListarInformacoesEscola_QuandoEscolaNaoForEncontrada_DeveRetornarHttpNotFound()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            escolaServiceMock.Setup(service => service.Listar(It.IsAny<int>())).Throws<InvalidOperationException>();

            int idEscola = 100;
            var result = controller.ListarInformacoesEscola(idEscola);

            escolaServiceMock.Verify(service => service.Listar(idEscola), Times.Once);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public void AdicionarSituacao_QuandoSituacaoForAdicionada_DeveRetornarOk()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            AtualizarSituacaoDTO atualizarSituacaoDto = new() { IdSituacao = 1, IdEscola = 2 };

            var result = controller.AdicionarSituacao(atualizarSituacaoDto);

            escolaServiceMock.Verify(service => service.AdicionarSituacao(atualizarSituacaoDto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void CadastrarEscola_QuandoEscolaForCadastrada_DeveRetornarHttpOk()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            EscolaStub escolaStub = new EscolaStub();
            var escola = escolaStub.ObterCadastroEscolaDTO();

            var result = controller.CadastrarEscola(escola);

            escolaServiceMock.Verify(service => service.CadastrarEscola(escola), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void RemoverSituacao_QuandoSituacaoForRemovida_DeveRetornarHttpOk()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            int idEscola = 1;
            var result = controller.RemoverSituacao(idEscola);

            escolaServiceMock.Verify(service => service.RemoverSituacaoEscola(idEscola), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
