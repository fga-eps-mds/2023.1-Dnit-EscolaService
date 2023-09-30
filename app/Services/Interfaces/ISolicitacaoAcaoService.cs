using api.Escolas;

namespace service.Interfaces
{
    public interface ISolicitacaoAcaoService
    {
        public void EnviarSolicitacaoAcao(SolicitacaoAcaoDTO solicitacaoAcaoDTO);
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo);
        public Task<IEnumerable<EscolaInep>> ObterEscolas(int municipio);
    }
}
