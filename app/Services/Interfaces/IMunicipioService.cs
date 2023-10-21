using api;
using api.Municipios;

namespace service.Interfaces
{
    public interface IMunicipioService
    {
        Task<IEnumerable<MunicipioModel>> ListarAsync(UF? uf);
    }
}
