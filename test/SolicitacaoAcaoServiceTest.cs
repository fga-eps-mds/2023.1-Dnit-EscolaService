using Moq;
using service;
using service.Interfaces;
using System.Net;
using System.Net.Mail;
using test.Stub;
using Moq.Protected;

namespace test
{
    public class SolicitacaoAcaoServiceTest
    {
        public SolicitacaoAcaoServiceTest()
        {
            Environment.SetEnvironmentVariable("EMAIL_SERVICE_ADDRESS", "teste_email@exemplo.com");
            Environment.SetEnvironmentVariable("EMAIL_SERVICE_PASSWORD", "teste");
            Environment.SetEnvironmentVariable("EMAIL_DNIT", "teste_email@exemplo.com");
        }

        [Fact]
        public void EnviarSolicitacaoAcao_QuandoSolicitacaoForPassada_DeveEnviarMensagemEsperada()
        {
            Mock<ISmtpClientWrapper> smtpClientWrapperMock = new();
            Mock<IHttpClientFactory> httpClientFactoryMock = new();

            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object, httpClientFactoryMock.Object);

            SolicitacaoAcaoStub solicitacaoAcaoStub = new SolicitacaoAcaoStub();
            var solicitacaoAcaoDTO = solicitacaoAcaoStub.ObterSolicitacaoAcaoDTO();

            service.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);

            string mensagemEsperada =   "Nova solicitação de ação em escola.\n\n" +
                                        "Escola: Escola Teste\n" +
                                        "UF: DF\n" +
                                        "Municipio: Brasília\n" +
                                        "Nome do Solicitante: João Testador\n" +
                                        "Vínculo com a escola: Professor\n" +
                                        "Email: joao@email.com\n" +
                                        "Telefone: 123123123\n" +
                                        "Ciclos de ensino: \n" +
                                        "    > Ensino Médio\n" +
                                        "    > Ensino Fundamental\n" +
                                        "Quantidade de alunos: 503\n" +
                                        "Observações: Teste de Solicitação\n";

            smtpClientWrapperMock.Verify(wrapper =>
                wrapper.Send(It.Is<MailMessage>(msg => msg.Body == mensagemEsperada)), Times.Once);
        }

        [Fact]
        public void EnviarEmail_QuandoDestinatarioForPassado_DeveEnviarEmail()
        {
            Mock<ISmtpClientWrapper> smtpClientWrapperMock = new();
            Mock<IHttpClientFactory> httpClientFactoryMock = new();

            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object, httpClientFactoryMock.Object);

            string emailDestinatario = "dnit@email.com";
            string assunto = "Solicitação de ação";
            string corpo = "Nova solicitação de ação";

            service.EnviarEmail(emailDestinatario, assunto, corpo);

            smtpClientWrapperMock.Verify(wrapper =>
                wrapper.Send(It.Is<MailMessage>(msg =>
                    msg.Subject == assunto &&
                    msg.Body == corpo &&
                    msg.To.Contains(new MailAddress(emailDestinatario))
            )), Times.Once);
        }

        [Fact]
        public void EnviarEmail_QuandoDestinatarioForVazio_DeveLancarExcecao()
        {
            Mock<ISmtpClientWrapper> smtpClientWrapperMock = new();
            Mock<IHttpClientFactory> httpClientFactoryMock = new();

            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object, httpClientFactoryMock.Object);

            string emailDestinatario = "";
            string assunto = "Solicitação de ação";
            string corpo = "Nova solicitação de ação";

            Assert.Throws<ArgumentException>(() => service.EnviarEmail(emailDestinatario, assunto, corpo));
        }

        [Fact]
        public async Task ObterEscolas_QuandoRequisicaoForFeita_DeveRetornarListaDeEscolas()
        {
            Mock<ISmtpClientWrapper> smtpClientWrapperMock = new();

            var resposta = @"[2,[{
                ""cod"": 12345678,
                ""nome"": ""Escola A"",
                ""estado"": ""DF"",
                ""cidade"": 100
            }, {
                ""cod"": 87654321,
                ""nome"": ""Escola B"",
                ""estado"": ""DF"",
                ""cidade"": 100
            }]]";

            var handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(handlerMock.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resposta)
                })
                .Verifiable();

            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object, httpClientFactoryMock.Object);

            int municipio = 100;
            var escolas = await service.ObterEscolas(municipio);

            Assert.Equal(12345678, escolas.ElementAt(0).Cod);
            Assert.Equal("Escola A", escolas.ElementAt(0).Nome);
            Assert.Equal("DF", escolas.ElementAt(0).Estado);

            Assert.Equal(87654321, escolas.ElementAt(1).Cod);
            Assert.Equal("Escola B", escolas.ElementAt(1).Nome);
            Assert.Equal("DF", escolas.ElementAt(1).Estado);
        }
    }
}
