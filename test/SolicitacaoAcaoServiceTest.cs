using app.Controllers;
using dominio;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
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
        public void EnviarSolicitacaoAcao_QuandoEnvioDoEmailFalhar_DeveRetornarErro()
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

        [Fact]
        public async Task ObterEscolas_QuandoEscolasForemObtidas_DeveRetornarListaEscolas()
        {
            var solicitacaoAcaoServiceMock = new Mock<ISolicitacaoAcaoService>();

            var controller = new SolicitacaoAcaoController(solicitacaoAcaoServiceMock.Object);

            List<EscolaInep> listaEscolas = new List<EscolaInep>
            {
                new EscolaInep { Cod = 1, Estado = "SP", Nome = "Escola A" },
                new EscolaInep { Cod = 2, Estado = "SP", Nome = "Escola B" },
                new EscolaInep { Cod = 3, Estado = "SP", Nome = "Escola C" }
            };

            var task = Task.FromResult<IEnumerable<EscolaInep>>(listaEscolas);

            solicitacaoAcaoServiceMock.Setup(service => service.ObterEscolas(It.IsAny<int>())).Returns(task);

            int municipio = 1;
            var result = await controller.ObterEscolas(municipio);

            solicitacaoAcaoServiceMock.Verify(service => service.ObterEscolas(municipio), Times.Once);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EscolaInep>>(result);

            var listaRetornada = result as IEnumerable<EscolaInep>;
            Assert.Equal(listaEscolas, listaRetornada);
        }
    }
}
