//using app.Controllers;
//using dominio;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using service.Interfaces;
//using System.Threading.Tasks;
//using test.Stub;

//namespace test
//{
//    public class EscolaControllerTest
//    {
//        const int INTERNAL_SERVER_ERROR = 500;

//        [Fact]
//        public void ObterEscolas_QuandoMetodoForChamado_DeveRetornarListaDeEscolas()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();

//            var controller = new EscolaController(escolaServiceMock.Object);

//            var filtro = new PesquisaEscolaFiltro();
//            filtro.Pagina = 1;
//            filtro.TamanhoPagina = 2;
//            var result = controller.ObterEscolas(filtro);

//            escolaServiceMock.Verify(service => service.Obter(filtro), Times.Once);
//            Assert.IsType<OkObjectResult>(result);
//        }
//        [Fact]
//        public void Excluir_QuandoIdEscolaForPassado_DeveExcluirEscola()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();

//            var controller = new EscolaController(escolaServiceMock.Object);

//            int idEscola = 59;
//            var result = controller.ExcluirEscola(idEscola);

//            escolaServiceMock.Verify(service => service.ExcluirEscolaAsync(idEscola), Times.Once);
//            Assert.IsType<OkResult>(result);
//        }

//        [Fact]
//        public async Task CadastrarEscola_QuandoEscolaForCadastrada_DeveRetornarHttpOk()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();

//            var controller = new EscolaController(escolaServiceMock.Object);

//            EscolaStub escolaStub = new EscolaStub();
//            var escola = escolaStub.ObterCadastroEscolaDTO();

//            var result = await controller.CadastrarEscolaAsync(escola);

//            escolaServiceMock.Verify(service => service.CadastrarEscolaAsync(escola), Times.Once);
//            Assert.IsType<OkResult>(result);
//        }

//        [Fact]
//        public void RemoverSituacao_QuandoSituacaoForRemovida_DeveRetornarHttpOk()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();

//            var controller = new EscolaController(escolaServiceMock.Object);

//            int idEscola = 1;
//            var result = controller.RemoverSituacao(idEscola);

//            escolaServiceMock.Verify(service => service.RemoverSituacaoEscola(idEscola), Times.Once);
//            Assert.IsType<OkResult>(result);
//        }
//        [Fact]
//        public void AlterarDadosEscola_QuandoAlterarDadosDaEscola_DeveRetornarOK()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();

//            var controller = new EscolaController(escolaServiceMock.Object);

//            EscolaStub escolaStub = new EscolaStub();
//            var escola = escolaStub.ObterAtualizarDadosEscolaDTO();
//            var result = controller.AlterarDadosEscola(escola);


//            escolaServiceMock.Verify(service => service.AlterarDadosEscola(escola), Times.Once);
//            Assert.IsType<OkResult>(result);
//        }
//        [Fact]
//        public void AlterarDadosEscola_QuandoIdDoAtributoNaoExistir_DeveRetornarConflict()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();
//            var excecao = new Npgsql.PostgresException("", "", "", "23503");

//            escolaServiceMock.Setup(service => service.AlterarDadosEscola(It.IsAny<AtualizarDadosEscolaDTO>())).Throws(excecao);

//            var controller = new EscolaController(escolaServiceMock.Object);

//            EscolaStub escolaStub = new();
//            AtualizarDadosEscolaDTO atualizarDadosEscolaDTO = escolaStub.ObterAtualizarDadosEscolaDTO();

//            var resultado = controller.AlterarDadosEscola(atualizarDadosEscolaDTO);

//            escolaServiceMock.Verify(service => service.AlterarDadosEscola(atualizarDadosEscolaDTO), Times.Once);
//            var objeto = Assert.IsType<ConflictObjectResult>(resultado);
//        }
//        [Fact]
//        public void AlterarDadosEscola_QuandoEscolaNaoForAlterada_DeveRetornarErroInterno()
//        {
//            var escolaServiceMock = new Mock<IEscolaService>();
//            var excecao = new Npgsql.PostgresException("", "", "", "");

//            escolaServiceMock.Setup(service => service.AlterarDadosEscola(It.IsAny<AtualizarDadosEscolaDTO>())).Throws(excecao);

//            var controller = new EscolaController(escolaServiceMock.Object);

//            EscolaStub escolaStub = new();
//            AtualizarDadosEscolaDTO atualizarDadosEscolaDTO = escolaStub.ObterAtualizarDadosEscolaDTO();

//            var resultado = controller.AlterarDadosEscola(atualizarDadosEscolaDTO);

//            escolaServiceMock.Verify(service => service.AlterarDadosEscola(atualizarDadosEscolaDTO), Times.Once);

//            var objeto = Assert.IsType<ObjectResult>(resultado);
//            Assert.Equal(INTERNAL_SERVER_ERROR, objeto.StatusCode);
//        }
//    }
//}
