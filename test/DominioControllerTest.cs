using app.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using repositorio.Interfaces;

namespace test
{
    public class DominioControllerTest
    {
        [Fact]
        public void ObterListaUF_QuandoMetodoForChamado_DeveRetornarHttpOk()
        {
            var dominioRepositorioMock = new Mock<IDominioRepositorio>();

            var controller = new DominioController(dominioRepositorioMock.Object);

            var result = controller.ObterListaUF();

            dominioRepositorioMock.Verify(r => r.ObterUnidadeFederativa(), Times.Once);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ObterListaEtapasdeEnsino_QuandoMetodoForChamado_DeveRetornarHttpOk()
        {
            var dominioRepositorioMock = new Mock<IDominioRepositorio>();

            var controller = new DominioController(dominioRepositorioMock.Object);

            var result = controller.ObterListaEtapasdeEnsino();

            dominioRepositorioMock.Verify(r => r.ObterEtapasdeEnsino(), Times.Once);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ObterListaSituacao_QuandoMetodoForChamado_DeveRetornarHttpOk()
        {
            var dominioRepositorioMock = new Mock<IDominioRepositorio>();

            var controller = new DominioController(dominioRepositorioMock.Object);

            var result = controller.ObterListaSituacao();

            dominioRepositorioMock.Verify(r => r.ObterSituacao(), Times.Once);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ObterListaMunicipio_QuandoMetodoForChamado_DeveRetornarHttpOk()
        {
            var dominioRepositorioMock = new Mock<IDominioRepositorio>();

            var controller = new DominioController(dominioRepositorioMock.Object);

            int idUf = 1;
            var result = controller.ObterListaMunicipio(idUf);

            dominioRepositorioMock.Verify(r => r.ObterMunicipio(idUf), Times.Once);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
