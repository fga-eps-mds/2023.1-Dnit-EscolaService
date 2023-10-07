using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public class MunicipioRepositorio : IMunicipioRepositorio
    {
        private readonly AppDbContext dbContext;

        public MunicipioRepositorio(
            AppDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<Municipio> ObterPorIdAsync(int id)
        {
            return await dbContext.Municipios.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new ApiException(ErrorCodes.MunicipioNaoEncontrado);
        }

        public async Task<List<Municipio>> ListarAsync(UF? uf)
        {
            var query = dbContext.Municipios.AsQueryable();

            if (uf.HasValue)
            {
                query = query.Where(m => m.Uf == uf.Value);
            }

            return await query.OrderBy(m => m.Nome).ToListAsync();
        }
    }
}
