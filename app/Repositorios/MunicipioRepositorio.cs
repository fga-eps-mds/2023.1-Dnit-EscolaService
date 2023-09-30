using api;
using app.Entidades;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public interface IMunicipioRepositorio
    {
        Task<Municipio> ObterPorIdAsync(int id);
    }

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
            return await dbContext.Municipios.FirstAsync(x => x.Id == id)
                ?? throw new ApiException(ErrorCodes.MunicipioNaoEncontrado);
        }
    }
}
