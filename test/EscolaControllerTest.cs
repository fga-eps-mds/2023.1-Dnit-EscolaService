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
        [Fact]
        public void AlterarDadosEscola_QuandoAlterarDadosDaEscola_DeveRetornarOK()
        {
            var escolaServiceMock = new Mock<IEscolaService>();

            var controller = new EscolaController(escolaServiceMock.Object);

            int idEscola = 1;
            EscolaStub escolaStub = new EscolaStub();
            var escola = escolaStub.ObterAtualizarDadosEscolaDTO();
            var result = controller.AlterarDadosEscola(escola);


            escolaServiceMock.Verify(service => service.AlterarDadosEscola(escola), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
