using dominio;
using Moq;
using repositorio;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            List<string> listaVazia = new List<string>();
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
        public void AdicionarSituacaoEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() { IdSituacao = 1, IdEscola = 2 };

            escolaService.AdicionarSituacao(atualizarSituacaoDto);
            mockEscolaRepositorio.Verify(x => x.AdicionarSituacao(atualizarSituacaoDto.IdSituacao, atualizarSituacaoDto.IdEscola), Times.Once);
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
        public void Listar_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 22;

            escolaService.Listar(IdEscola);
            mockEscolaRepositorio.Verify(x => x.Obter(IdEscola), Times.Once);
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

        [Fact]
        public void AdicionarSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() { IdSituacao = 7, IdEscola = 4 };
            int IdInexistente = 3;

            escolaService.AdicionarSituacao(atualizarSituacaoDto);
            mockEscolaRepositorio.Verify(x => x.AdicionarSituacao(atualizarSituacaoDto.IdSituacao, IdInexistente), Times.Never);
        }

        [Fact]
        public void Listar_QuandoOIdInexistenteForChamado_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 22;
            var IdInexistente = 15;

            escolaService.Listar(IdEscola);
            mockEscolaRepositorio.Verify(x => x.Obter(IdInexistente), Times.Never);
        }

        [Fact]
        public void ObterCodigoMunicipioPorCEP_QuandoCEPNullForPassado_DeveRetornarNull()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            string cep = null;
            var codigo = escolaService.ObterCodigoMunicipioPorCEP(cep).GetAwaiter().GetResult();

            Assert.Null(codigo);
        }

        [Fact]
        public void ObterCodigoMunicipioPorCEP_QuandoCEPInvalidoForPassado_DeveRetornarNull()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            string cep = "cep invalido";
            var codigo = escolaService.ObterCodigoMunicipioPorCEP(cep).GetAwaiter().GetResult();

            Assert.Null(codigo);
        }

        [Fact]
        public void ObterCodigoMunicipioPorCEP_QuandoCEPValidoForPassado_DeveRetornarValorReal()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            string cep = "71687214";
            string codigo_brasiilia = "5300108";
            var codigo = escolaService.ObterCodigoMunicipioPorCEP(cep).GetAwaiter().GetResult();

            Assert.Equal(codigo_brasiilia, codigo);
        }

        [Fact]
        public void ObterEstadoPelaSigla_QuandoSiglaValidaForPassada_DeveRetornarIdCorreto()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string sigla = "AC";
            int idCorreto = 1;
            var id = escolaService.ObterEstadoPelaSigla(sigla);

            Assert.Equal(idCorreto, id);

            sigla = "MG";
            idCorreto = 12;
            id = escolaService.ObterEstadoPelaSigla(sigla);

            Assert.Equal(idCorreto, id);
        }

        [Fact]
        public void ObterEstadoPelaSigla_QuandoSiglaValidaMinusculaForPassada_DeveRetornarIdCorreto()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string sigla = "pB";
            int idCorreto = 14;
            var id = escolaService.ObterEstadoPelaSigla(sigla);

            Assert.Equal(idCorreto, id);

            sigla = "df";
            idCorreto = 27;
            id = escolaService.ObterEstadoPelaSigla(sigla);

            Assert.Equal(idCorreto, id);
        }

        [Fact]
        public void ObterEstadoPelaSigla_QuandoSiglaInvalidaForPassada_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string sigla = "xx";
            int idCorreto = 0;
            var id = escolaService.ObterEstadoPelaSigla(sigla);

            Assert.Equal(idCorreto, id);
        }

        [Fact]
        public void ObterPortePeloId_QuandoPorteCorretoForPassado_DeveRetornarIdCorreto()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string porte1 = "Até 50 matrículas de escolarização";
            var id1 = escolaService.ObterPortePeloId(porte1);
            int idPorte1 = 1;

            string porte2 = "Entre 201 e 500 matrículas de escolarização";
            var id2 = escolaService.ObterPortePeloId(porte2);
            int idPorte2 = 2;

            string porte3 = "Entre 501 e 1000 matrículas de escolarização";
            var id3 = escolaService.ObterPortePeloId(porte3);
            int idPorte3 = 3;

            string porte4 = "Entre 51 e 200 matrículas de escolarização";
            var id4 = escolaService.ObterPortePeloId(porte4);
            int idPorte4 = 4;

            string porte5 = "Mais de 1000 matrículas de escolarização";
            var id5 = escolaService.ObterPortePeloId(porte5);
            int idPorte5 = 5;

            Assert.Equal(idPorte1, id1);
            Assert.Equal(idPorte2, id2);
            Assert.Equal(idPorte3, id3);
            Assert.Equal(idPorte4, id4);
            Assert.Equal(idPorte5, id5);
        }

        [Fact]
        public void ObterPortePeloId_QuandoPassadoPorteInvalido_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string porte = "Porte teste";
            int id = escolaService.ObterPortePeloId(porte);

            Assert.Equal(0, id);
        }

        [Fact]
        public void SuperaTamanhoMaximo_QuandoPlanilhaComTamanhoMaiorQueOMaximoForPassada_DeveRetornarTrue()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string caminhoArquivo = "../../../Stub/planilha_maior_max.csv";

            MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(caminhoArquivo));

            bool resultado = escolaService.SuperaTamanhoMaximo(memoryStream);

            Assert.True(resultado);
        }

        [Fact]
        public void SuperaTamanhoMaximo_QuandoPlanilhaComTamanhoMenorQueOMaximoForPassada_DeveRetornarFalse()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string caminhoArquivo = "../../../Stub/planilha_menor_max.csv";

            MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(caminhoArquivo));

            bool resultado = escolaService.SuperaTamanhoMaximo(memoryStream);

            Assert.False(resultado);
        }

        [Fact]
        public void EtapasParaIds_QuandoNenhumaEtapaForPassada_DeveRetornarException()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string etapas = "";
            string nome = "Nome escola";

            Assert.Throws<Exception>(() => escolaService.EtapasParaIds(etapas, nome));
        }

        [Fact]
        public void EtapasParaIds_QuandoAlgumaEtapaErradaForPassada_DeveRetornarException()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string etapas = "Educação infantil, ensino errado";
            string nome = "Nome escola";

            Assert.Throws<Exception>(() => escolaService.EtapasParaIds(etapas, nome));
        }

        [Fact]
        public void EtapasParaIds_QuandoEtapaComLetraMinusculaForPassada_DeveRetornarListaComTamanhoIgualAQuantidadeDeEtapasPassadas()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string etapas = "Educação Infantil, EDUCAÇÃO profissional";
            int quantidade_etapas = etapas.Split(',').Select(item => item.Trim()).ToList().Count;

            string nome = "Nome escola";
            int quantidade_ids = escolaService.EtapasParaIds(etapas, nome).Count;

            Assert.Equal(quantidade_etapas, quantidade_ids);
        }

        [Fact]
        public void ObterRedePeloId_QuandoRedeErradaForPassada_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string rede = "erro";
            int id = 0;

            Assert.Equal(id, escolaService.ObterRedePeloId(rede));
        }

        [Fact]
        public void ObterRedePeloId_QuandoNenhumaRedeForPassada_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string rede = "";
            int id = 0;

            Assert.Equal(id, escolaService.ObterRedePeloId(rede));
        }

        [Fact]
        public void ObterRedePeloId_QuandoRedesCorretasForemPassadas_DeveRetornarIdCerto()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string rede1 = "municipal";
            int id1 = 1;

            string rede2 = "estadual";
            int id2 = 2;

            string rede3 = "privada";
            int id3 = 3;

            Assert.Equal(id1, escolaService.ObterRedePeloId(rede1));
            Assert.Equal(id2, escolaService.ObterRedePeloId(rede2));
            Assert.Equal(id3, escolaService.ObterRedePeloId(rede3));
        }

        [Fact]
        public void ObterLocalizacaoPeloId_QuandoLocalizacaoCorretaForPassada_DeveRetornarIdCerto()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string loc1 = "rural";
            int id1 = 1;

            string loc2 = "urbana";
            int id2 = 2;

            Assert.Equal(id1, escolaService.ObterLocalizacaoPeloId(loc1));
            Assert.Equal(id2, escolaService.ObterLocalizacaoPeloId(loc2));
        }

        [Fact]
        public void ObterLocalizacaoPeloId_QuandoLocalizacaoErradaForPassada_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string loc = "erro";
            int id = 0;

            Assert.Equal(id, escolaService.ObterLocalizacaoPeloId(loc));
        }

        [Fact]
        public void ObterLocalizacaoPeloId_QuandoNenhumaLocalizacaoForPassada_DeveRetornarZero()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            string loc = "";
            int id = 0;

            Assert.Equal(id, escolaService.ObterRedePeloId(loc));
        }
    }
}