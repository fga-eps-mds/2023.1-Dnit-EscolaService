using dominio;
using Moq;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using test.Stub;

namespace test
{
    public class EscolaServiceTest
    {
        [Fact]
        public void ExcluirEscola_QuandoForChamado_DeveChamarExcluirEscolaDoRepositorio()
        {
            Mock<IEscolaRepositorio> escolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(escolaRepositorio.Object);
            int idEscolaTest = 41;

            escolaService.ExcluirEscola(idEscolaTest);
            escolaRepositorio.Verify(x => x.ExcluirEscola(idEscolaTest), Times.Once);
        }
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
            mockEscolaRepositorio.Verify(mock => mock.CadastrarEscola(It.IsAny<Escola>()), Times.Never);
        }

        [Fact]
        public void CadastrarEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            EscolaStub escolaStub = new();
            CadastroEscolaDTO cadastroEscolaDTO = escolaStub.ObterCadastroEscolaDTO();
            int idEscola = 1;
            mockEscolaRepositorio.Setup(repositorio => repositorio.CadastrarEscola(cadastroEscolaDTO)).Returns(idEscola);


            escolaService.CadastrarEscola(cadastroEscolaDTO);
            mockEscolaRepositorio.Verify(x => x.CadastrarEscola(cadastroEscolaDTO), Times.Once);
            mockEscolaRepositorio.Verify(x => x.CadastrarEtapasDeEnsino(idEscola, cadastroEscolaDTO.IdEtapasDeEnsino[0]), Times.Once);
        }
        [Fact]
        public void CadastrarEscola_QuandoCadastroFalhar_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            EscolaStub escolaStub = new();
            CadastroEscolaDTO cadastroEscolaDTO = escolaStub.ObterCadastroEscolaDTO();
            int idEscola = 0;
            mockEscolaRepositorio.Setup(repositorio => repositorio.CadastrarEscola(cadastroEscolaDTO)).Returns(idEscola);


            Action cadastrarEscola = () => escolaService.CadastrarEscola(cadastroEscolaDTO);
            Assert.Throws<Exception>(cadastrarEscola);
            mockEscolaRepositorio.Verify(x => x.CadastrarEscola(cadastroEscolaDTO), Times.Once);
            mockEscolaRepositorio.Verify(x => x.CadastrarEtapasDeEnsino(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Obter_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            PesquisaEscolaFiltro pesquisaEscolaFiltro = new() { Pagina = 1, TamanhoPagina = 2 };

            escolaService.Obter(pesquisaEscolaFiltro);
            mockEscolaRepositorio.Verify(x => x.ObterEscolas(pesquisaEscolaFiltro), Times.Once);
        }
        [Fact]
        public void AlterarDadosEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarDadosEscolaDTO atualizarDadosEscolaDto = new() { IdSituacao = 1, IdEscola = 2 };

            escolaService.AlterarDadosEscola(atualizarDadosEscolaDto);
            mockEscolaRepositorio.Verify(x => x.AlterarDadosEscola(atualizarDadosEscolaDto), Times.Once);
        }

        [Fact]
        public void RemoverSituacaoEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 5;

            escolaService.RemoverSituacaoEscola(IdEscola);
            mockEscolaRepositorio.Verify(x => x.RemoverSituacaoEscola(IdEscola), Times.Once);
        }

        [Fact]
        public void RemoverSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 8;
            var IdInexistente = 999;

            escolaService.RemoverSituacaoEscola(IdInexistente);
            mockEscolaRepositorio.Verify(x => x.RemoverSituacaoEscola(IdInexistente), Times.Once);
        }
    }
}