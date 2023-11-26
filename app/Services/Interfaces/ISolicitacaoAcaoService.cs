using api.Escolas;

namespace service.Interfaces
{
    public interface ISolicitacaoAcaoService
    {
        public void EnviarSolicitacaoAcao(SolicitacaoAcaoData solicitacaoAcaoDTO);
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo);
        public Task<IEnumerable<EscolaInep>> ObterEscolas(int municipio);
        public Task Criar(SolicitacaoAcaoData solicitacao);
    }
}
