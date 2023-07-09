using app.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using System.Net.Mail;
using test.Stub;

namespace test
{
    public class SolicitacaoAcaoControllerTest
    {
        const int INTERNAL_SERVER_ERROR = 500;

        [Fact]
        public void EnviarSolicitacaoAcao_QuandoSolicitacaoForEnviada_DeveRetornarOk()
        {
            SolicitacaoAcaoStub solicitacaoAcaoStub = new SolicitacaoAcaoStub();
            var solicitacaoAcaoDTO = solicitacaoAcaoStub.ObterSolicitacaoAcaoDTO();

            var solicitacaoAcaoServiceMock = new Mock<ISolicitacaoAcaoService>();

            var controller = new SolicitacaoAcaoController(solicitacaoAcaoServiceMock.Object);

            var result = controller.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);

            solicitacaoAcaoServiceMock.Verify(service => service.EnviarSolicitacaoAcao(solicitacaoAcaoDTO), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EnviarSolicitacaoAcao_QuandoEnvioDoEmailFalar_DeveRetornarErro()
        {
            SolicitacaoAcaoStub solicitacaoAcaoStub = new SolicitacaoAcaoStub();
            var solicitacaoAcaoDTO = solicitacaoAcaoStub.ObterSolicitacaoAcaoDTO();

            var solicitacaoAcaoServiceMock = new Mock<ISolicitacaoAcaoService>();
            solicitacaoAcaoServiceMock.Setup(x => x.EnviarSolicitacaoAcao(solicitacaoAcaoDTO)).Throws<SmtpException>();

            var controller = new SolicitacaoAcaoController(solicitacaoAcaoServiceMock.Object);

            var result = controller.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);

            solicitacaoAcaoServiceMock.Verify(service => service.EnviarSolicitacaoAcao(solicitacaoAcaoDTO), Times.Once);
            var objeto = Assert.IsType<ObjectResult>(result);
            Assert.Equal(INTERNAL_SERVER_ERROR, objeto.StatusCode);
        }
    }
}
