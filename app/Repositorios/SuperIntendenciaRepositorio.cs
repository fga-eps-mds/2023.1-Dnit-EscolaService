using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios;

public class SuperIntendenciaRepositorio : ISuperintendenciaRepositorio
{

    private readonly AppDbContext _dbContext;

    public SuperIntendenciaRepositorio(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Superintendencia> ObterPorIdAsync(int id)
    {
        return await _dbContext.Superintendencias.FirstOrDefaultAsync(x => x.Id == id)
               ?? throw new ApiException(ErrorCodes.SuperIntendenciaNaoEncontrada);
    }

    public async Task<List<Superintendencia>> ListarAsync(UF? uf)
    {
        var query = _dbContext.Superintendencias.AsQueryable();

        if (uf.HasValue)
        {
            query = query.Where(m => m.Uf == uf.Value);
        }

        return await query.ToListAsync();

    }

    public async Task<List<Superintendencia>> ListarAsync()
    {
        return await _dbContext.Superintendencias.ToListAsync();
    }
}
