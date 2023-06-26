using Xunit;
using Moq;
using repositorio.Interfaces;
using service;
using dominio;
using service.Interfaces;

namespace test
{
    public class EscolaRepositorioTest
    {
        [Fact]
        public void AdicionarSituacaoEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new ();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() {IdSituacao = 1, IdEscola = 2};

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
            mockEscolaRepositorio.Verify(x => x.RemoverSituacaoEscola(IdInexistente),Times.Once);
        }

         [Fact]
        public void AdicionarSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() {IdSituacao = 7, IdEscola = 4};
            int IdInexistente = 3;

            escolaService.AdicionarSituacao(atualizarSituacaoDto);
            mockEscolaRepositorio.Verify(x => x.AdicionarSituacao(atualizarSituacaoDto.IdSituacao, IdInexistente),Times.Never);
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
    }
}