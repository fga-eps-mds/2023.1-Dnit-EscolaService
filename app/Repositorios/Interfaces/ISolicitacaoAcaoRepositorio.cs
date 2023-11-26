using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface ISolicitacaoAcaoRepositorio
    {
        public Task<SolicitacaoAcao> Criar(SolicitacaoAcaoData solicitacao);
        public Task<SolicitacaoAcao?> ObterPorEscolaIdAsync(Guid escolaId);
    }
}
