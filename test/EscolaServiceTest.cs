using dominio;
using Moq;
using repositorio.Interfaces;
using service;
using service.Interfaces;

namespace test
{
    public class EscolaServiceTest
    {
        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoPlanilhaVaziaForPassada_DeveRetornarListaVazia()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            List<int> listaVazia = new List<int>();
            var memoryStream = new MemoryStream();

            var retorno = escolaService.CadastrarEscolaViaPlanilha(memoryStream);

            Assert.Equal(retorno, listaVazia);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoPlanilhaForPassada_DevePassarPeloRepositorio()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var memoryStream = new MemoryStream();

            var retorno = escolaService.CadastrarEscolaViaPlanilha(memoryStream);
            mockEscolaRepositorio.Verify(mock => mock.CadastrarEscola(It.IsAny<Escola>()), Times.Once);
        }
    }
}