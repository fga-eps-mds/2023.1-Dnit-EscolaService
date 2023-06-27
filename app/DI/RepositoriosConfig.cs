using repositorio;
using repositorio.Interfaces;

namespace app.DI
{
    public static class RepositoriosConfig
    {
        public static void AddConfigRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IEscolaRepositorio, EscolaRepositorio>();
            services.AddScoped<IDominioRepositorio, DominioRepositorio>();
        }
    }
}
