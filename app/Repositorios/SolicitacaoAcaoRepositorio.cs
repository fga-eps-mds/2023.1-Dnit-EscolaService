using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;

namespace app.Repositorios
{
    public class SolicitacaoAcaoRepositorio : ISolicitacaoAcaoRepositorio
    {
        private readonly AppDbContext dbContext;

        public SolicitacaoAcaoRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public SolicitacaoAcao Criar(SolicitacaoAcaoData s)
        {
            var sol = new SolicitacaoAcao
            {
                Email = s.Email,
                Telefone = s.Telefone,
                NomeSolicitante = s.NomeSolicitante,
                Observacoes = s.Observacoes,
                // FIXME: como pegar esse Id?
                EscolaId = Guid.NewGuid(),
                DataRealizada = DateTimeOffset.Now
            };

            dbContext.Solicitacoes.Add(sol);
            return sol;
        }
    }
}