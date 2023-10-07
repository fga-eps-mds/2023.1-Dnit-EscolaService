using service.Interfaces;
using api;
using app.Repositorios.Interfaces;
using app.Services;
using api.Municipios;

namespace app.service
{
    public class MunicipioService : IMunicipioService
    {
        private readonly IMunicipioRepositorio municipioRepositorio;
        private readonly ModelConverter modelConverter;

        public MunicipioService(
            IMunicipioRepositorio municipioRepositorio,
            ModelConverter modelConverter
        )
        {
            this.municipioRepositorio = municipioRepositorio;
            this.modelConverter = modelConverter;
        }

        public async Task<IEnumerable<MunicipioModel>> ListarAsync(UF? uf)
        {
            var municipios = await municipioRepositorio.ListarAsync(uf);
            return municipios.Select(modelConverter.ToModel);
        }
    }
}


