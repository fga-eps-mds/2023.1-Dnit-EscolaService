using Moq;
using service;
using service.Interfaces;
using System;
using System.Net.Mail;
using test.Stub;
using Xunit;

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
            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object);

            SolicitacaoAcaoStub solicitacaoAcaoStub = new SolicitacaoAcaoStub();
            var solicitacaoAcaoDTO = solicitacaoAcaoStub.ObterSolicitacaoAcaoDTO();

            service.EnviarSolicitacaoAcao(solicitacaoAcaoDTO);

            string mensagemEsperada = "Nova solicitação de ação em escola.\n\n" +
                                      "Escola: Escola Teste\n" +
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
            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object);

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
            ISolicitacaoAcaoService service = new SolicitacaoAcaoService(smtpClientWrapperMock.Object);

            string emailDestinatario = "";
            string assunto = "Solicitação de ação";
            string corpo = "Nova solicitação de ação";

            Assert.Throws<ArgumentException>(() => service.EnviarEmail(emailDestinatario, assunto, corpo));
        }
    }
}
