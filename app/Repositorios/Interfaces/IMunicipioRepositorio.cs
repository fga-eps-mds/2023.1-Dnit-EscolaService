using api;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IMunicipioRepositorio
    {
        Task<Municipio> ObterPorIdAsync(int id);
        Task<List<Municipio>> ListarAsync(UF? uf);
    }
}