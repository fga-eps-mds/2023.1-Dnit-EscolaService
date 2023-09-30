using app.Entidades;
using app.service;
using Microsoft.EntityFrameworkCore;
using service.Interfaces;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql")));

            services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
            services.AddScoped<ISolicitacaoAcaoService, SolicitacaoAcaoService>();
            services.AddScoped<IEscolaService, EscolaService>();

            services.AddControllers(o => o.Filters.Add(typeof(HandleExceptionFilter)));

            services.AddHttpClient();
        }
    }
}
