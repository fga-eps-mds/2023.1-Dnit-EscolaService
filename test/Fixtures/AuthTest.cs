using app.Controllers;
using app.Entidades;
using app.Repositorios;
using app.Repositorios.Interfaces;
using app.Services;
using auth;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test.Fixtures
{
    public class Base : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            // Para evitar a colisão durante a testagem paralela, o nome deve ser diferente para cada classe de teste
            var databaseName = "DbInMemory" + Random.Shared.Next().ToString();
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(databaseName));

            // Repositorios
            services.AddScoped<IEscolaRepositorio, EscolaRepositorio>();
            services.AddScoped<IMunicipioRepositorio, MunicipioRepositorio>();
            services.AddScoped<IRanqueRepositorio, RanqueRepositorio>();

            // Services
            services.AddScoped<IEscolaService, EscolaService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<ISolicitacaoAcaoService, SolicitacaoAcaoService>();
            services.AddScoped<IRanqueService, RanqueService>();
            services.AddSingleton<ModelConverter>();

            // Controllers
            services.AddScoped<DominioController>();
            services.AddScoped<EscolaController>();
            services.AddScoped<RanqueController>();

            services.AddAuth(configuration);
        }

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.Test.json", IsOptional = false };
        }
    }
}
