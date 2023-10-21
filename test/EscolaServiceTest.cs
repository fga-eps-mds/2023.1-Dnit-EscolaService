using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Fixtures;
using test.Stubs;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class EscolaServiceTest : TestBed<Base>
    {
        IEscolaService escolaService;
        IEscolaRepositorio escolaRepositorio;
        AppDbContext dbContext;

        public EscolaServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            dbContext.PopulaEscolas(5);

            escolaService = fixture.GetService<IEscolaService>(testOutputHelper)!;
            escolaRepositorio = fixture.GetService<IEscolaRepositorio>(testOutputHelper)!;
        }

        [Fact]
        public async Task ExcluirEscola_QuandoForChamado_DeveChamarExcluirEscolaDoRepositorio()
        {
            var escola = dbContext.Escolas.First();

            await escolaService.ExcluirAsync(escola.Id);
            Assert.False(dbContext.Escolas.Contains(escola));
        }


        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoPlanilhaVaziaForPassada_DeveRetornarListaVazia()
        {
            var memoryStream = new MemoryStream();

            var retorno = await escolaService.CadastrarAsync(memoryStream);

            Assert.Empty(retorno);
        }

        [Fact]
        public void RemoverSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            var escola = dbContext.Escolas.First();

            escolaService.RemoverSituacaoAsync(escola.Id);
            var escolaDb = dbContext.Escolas.First();

            Assert.Equal(escolaDb.Id, escola.Id);
            Assert.Null(escolaDb.Situacao);
        }

        [Fact]
        public void CadastrarAsync_QuandoPlanilhaComTamanhoMaiorQueOMaximoForPassada_DeveRetornarTrue()
        {
            var caminhoArquivo = Path.Join("..", "..", "..", "Stubs", "planilha_maior_max.csv");
            var stream = new MemoryStream(File.ReadAllBytes(caminhoArquivo));
            var resultado = escolaService.SuperaTamanhoMaximo(stream);
            Assert.True(resultado);
        }

        [Fact]
        public void SuperaTamanhoMaximo_QuandoPlanilhaComTamanhoMenorQueOMaximoForPassada_DeveRetornarFalse()
        {
            var caminhoArquivo = Path.Join("..", "..", "..", "Stubs", "planilha_menor_max.csv");
            var stream = new MemoryStream(File.ReadAllBytes(caminhoArquivo));
            var resultado = escolaService.SuperaTamanhoMaximo(stream);
            Assert.False(resultado);

        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoCepInvalidoForPassado_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;cep_errado;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, CEP inválido! (Parameter 'Cep')", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoRedeInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;rede errada;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, rede inválida! (Parameter 'Rede')", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoUFInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;xx;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, UF inválida! (Parameter 'Uf')", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoLocalizacaoInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;localizacao errada;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, localização inválida! (Parameter 'Localizacao')", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoPorteInvalidoForPassado_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;41127226;ANISIO TEIXEIRA E M EF;Municipal;porte errado;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Erro. A leitura do arquivo parou na escola: ANISIO TEIXEIRA E M EF, descrição do porte inválida! (Parameter 'Porte')", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoPlanilhaInvalidaForPassada_DeveRetornarExceptionComMensagem()
        {
            var planilha = new StringBuilder();
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine("2019;1;codigo errado;ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var exception = await Assert.ThrowsAsync<FormatException>(() => escolaService.CadastrarAsync(memoryStream));
            Assert.Equal("Planilha com formato incompatível.", exception.Message);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoEscolasJaCadastradasNoBancoForemPassadas_DeveAtualizarOsDados()
        {
            var planilha = new StringBuilder();
            var codigo = 41127226;
            var nome = "ANISIO TEIXEIRA E M EF 2";
            var rede = Rede.Privada;
            var porte = Porte.Entre501e1000;
            var endereco = "RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 BSB - DF.";
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine($"2019;1;{codigo};ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");
            planilha.AppendLine($"2019;1;{codigo};{nome};{rede};{porte.AsString(EnumFormat.Description)};{endereco};82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;Ensino Fundamental, Educação de Jovens Adultos;;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            await escolaService.CadastrarAsync(memoryStream);
            var escola = (await escolaRepositorio.ObterPorCodigoAsync(codigo))!;

            Assert.Equal(nome, escola.Nome);
            Assert.Equal(rede, escola.Rede);
            Assert.Equal(porte, escola.Porte);
            Assert.Equal(endereco, escola.Endereco);
        }

        [Fact]
        public async Task CadastrarEscolaViaPlanilha_QuandoEtapasDasEscolasForemCadastradas_DeveEstarInclusaNaEscola()
        {
            var planilha = new StringBuilder();
            var escolaCodigo = 41127226;
            var etapas = new List<EtapaEnsino>() { EtapaEnsino.Fundamental, EtapaEnsino.JovensAdultos };
            var descricoesEtapa = string.Join(',', etapas.Select(e => e.AsString(EnumFormat.Description)));
            planilha.AppendLine("Ano do Censo Escolar;ID;Cod. INEP;Nome da Instituição de Ensino;Rede;Porte da Instituição de Ensino;Endereço;CEP;Cidade;UF;Localização;Latitude;Longitude;DDD;Telefone da instituição;Etapas de Ensino Contempladas;Nº de Matrículas Ensino Infantil;Nº de Matrículas 1º ano Ensino Fundamental;Nº de Matrículas 2º ano Ensino Fundamental;Nº de Matrículas 3º ano Ensino Fundamental;Nº de Matrículas 4º ano Ensino Fundamental;Nº de Matrículas 5º ano Ensino Fundamental;Nº de Matrículas 6º ano Ensino Fundamental;Nº de Matrículas 7º ano Ensino Fundamental;Nº de Matrículas 8º ano Ensino Fundamental;Nº de Matrículas 9º ano Ensino Fundamental;Nº de Docentes");
            planilha.AppendLine($"2019;1;{escolaCodigo};ANISIO TEIXEIRA E M EF;Municipal;Entre 201 e 500 matrículas de escolarização;RUA JOAO BATISTA SCUCATO, 80 ATUBA. 82860-130 Curitiba - PR.;82860130;Curitiba;PR;Urbana;-25,38443;-49,2011;41;32562393;{descricoesEtapa};;70;90;92;65;73;0;0;0;0;126\r\n");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(planilha.ToString()));

            var retorno = await escolaService.CadastrarAsync(memoryStream);
            var escola = (await escolaRepositorio.ObterPorCodigoAsync(escolaCodigo, incluirEtapas: true))!;

            Assert.Equal(etapas.Count, escola.EtapasEnsino?.Count);
            Assert.True(etapas.All(e => escola.EtapasEnsino?.Exists(ee => ee.EtapaEnsino == e) ?? false));
        }

        public new void Dispose()
        {
            dbContext.Clear();
        }
    }
}