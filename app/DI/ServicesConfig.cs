using auth;
using app.Entidades;
using app.Services;
using Microsoft.EntityFrameworkCore;
using service.Interfaces;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mode = Environment.GetEnvironmentVariable("MODE");
            var connectionString = mode == "container" ? "PostgreSqlDocker" : "PostgreSql";

            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionString)));

            services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
            services.AddSingleton<ModelConverter>();

            services.AddScoped<IEscolaService, EscolaService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<ISolicitacaoAcaoService, SolicitacaoAcaoService>();
            services.AddScoped<IRanqueService, RanqueService>();

            services.AddControllers(o => o.Filters.Add(typeof(HandleExceptionFilter)));

            services.AddHttpClient();
            services.AddAuth(configuration);

            // appsettings.Development.json
                // "Hangfire": "Host=localhost;Port=5333;Database=upsjobs;Username=postgres;Password=1234",
                // "HangfireDocker": "Host=dnit-ups-db;Port=5432;Database=upsjobs;Username=postgres;Password=1234"

            // var conexaoHangfire = mode == "container" ? "HangfireDocker" : "Hangfire";
            // services.AddHangfire(config => config
            //     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            //     .UseSimpleAssemblyNameTypeSerializer()
            //     .UseRecommendedSerializerSettings()
            //     .UsePostgreSqlStorage(c =>
            //         c.UseNpgsqlConnection(configuration.GetConnectionString(conexaoHangfire)))
            // );
            // services.AddHangfireServer();
            
            // precisa mesmo ou é só um exemplo???
            // services.AddMvc();
        }
    }
}
