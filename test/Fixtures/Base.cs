using app.Entidades;
using app.Repositorios;
using app.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using repositorio;
using repositorio.Interfaces;
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
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("DbInMemory"));

            // Repositorios
            services.AddScoped<IEscolaRepositorio, app.Repositorios.EscolaRepositorio>();
            services.AddScoped<IDominioRepositorio, DominioRepositorio>();

            // Services
            services.AddScoped<IEscolaService, EscolaService>();
            services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
            services.AddScoped<ISolicitacaoAcaoService, SolicitacaoAcaoService>();
            services.AddScoped<IEscolaService, EscolaService>();

        }

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }
    }
}
