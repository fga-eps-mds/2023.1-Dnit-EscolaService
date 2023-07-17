using dominio;
using Moq;
using repositorio;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            string nome = "Nome escola";

            string porte1 = "Até 50 matrículas de escolarização";
            var id1 = escolaService.ObterPortePeloId(porte1, nome);
            int idPorte1 = 1;

            string porte2 = "Entre 201 e 500 matrículas de escolarização";
            var id2 = escolaService.ObterPortePeloId(porte2, nome);
            int idPorte2 = 2;

            string porte3 = "Entre 501 e 1000 matrículas de escolarização";
            var id3 = escolaService.ObterPortePeloId(porte3, nome);
            int idPorte3 = 3;

            string porte4 = "Entre 51 e 200 matrículas de escolarização";
            var id4 = escolaService.ObterPortePeloId(porte4, nome);
            int idPorte4 = 4;

            string porte5 = "Mais de 1000 matrículas de escolarização";
            var id5 = escolaService.ObterPortePeloId(porte5, nome);
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

            string porte = "Porte para o teste";
            string nome = "Nome escola";
            int id = escolaService.ObterPortePeloId(porte, nome);

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

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoCepInvalidoForPassado_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;cep_errado;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, CEP invalido!", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoRedeInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;rede errada;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, rede invalida!", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoUFInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;xx;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, UF invalida!", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoLocalizacaoInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;localizacao errada;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, localizacao invalida!", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoPorteInvalidoForPassado_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;porte errado;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, descricao do porte invalida!", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoPlanilhaInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;codigo errado;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            Exception exception = Assert.Throws<Exception>(() => escolaService.CadastrarEscolaViaPlanilha(memoryStream));
            Assert.Equal("Planilha com formato incompatível.", exception.Message);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoEscolasJaCadastradasNoBancoForemPassadas_DevePassarPeloRepositorio()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var retorno = escolaService.CadastrarEscolaViaPlanilha(memoryStream);
            mockEscolaRepositorio.Verify(mock => mock.AtualizarDadosPlanilha(It.IsAny<Escola>()), Times.Never);
        }

        [Fact]
        public void CadastrarEscolaViaPlanilha_QuandoEtapasDasEscolasForemCadastradas_DevePassarPeloRepositorio2Vezes()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);

            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var retorno = escolaService.CadastrarEscolaViaPlanilha(memoryStream);
            mockEscolaRepositorio.Verify(mock => mock.CadastrarEtapasDeEnsino(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }
    }
}