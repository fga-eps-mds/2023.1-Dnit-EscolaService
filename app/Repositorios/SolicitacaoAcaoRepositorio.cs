using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public class SolicitacaoAcaoRepositorio : ISolicitacaoAcaoRepositorio
    {
        private readonly AppDbContext dbContext;

        public SolicitacaoAcaoRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SolicitacaoAcao> Criar(SolicitacaoAcaoData s)
        {
            var sol = new SolicitacaoAcao
            {
                EscolaId = s.EscolaId,
                Email = s.Email,
                Telefone = s.Telefone,
                NomeSolicitante = s.NomeSolicitante,
                DataRealizada = DateTimeOffset.Now,
                Observacoes = s.Observacoes,
            };
            await dbContext.Solicitacoes.AddAsync(sol);
            return sol;
        }

        public async Task<SolicitacaoAcao?> ObterPorEscolaIdAsync(Guid escolaId)
        {
            return await dbContext.Solicitacoes.Where(e => e.EscolaId == escolaId).FirstOrDefaultAsync();
        }
    }
}