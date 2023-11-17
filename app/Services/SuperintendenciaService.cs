using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;

namespace app.Services;

public class SuperintendenciaService : ISuperintendenciaService
{
    private readonly ISuperintendenciaRepositorio _superintendenciaRepositorio;

    public SuperintendenciaService(ISuperintendenciaRepositorio superintendenciaRepositorio)
    {
        _superintendenciaRepositorio = superintendenciaRepositorio;
    }

    public async Task<Superintendencia> ObterPorIdAsync(int id)
    {
        return await _superintendenciaRepositorio.ObterPorIdAsync(id);
    }
}
