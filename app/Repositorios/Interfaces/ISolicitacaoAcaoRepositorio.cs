using api.Escolas;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface ISolicitacaoAcaoRepositorio
    {
        SolicitacaoAcao Criar(SolicitacaoAcaoData solicitacao);
    }
}
