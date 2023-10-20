using auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using service;
using service.Interfaces;
using System.Text;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
            services.AddScoped<ISolicitacaoAcaoService, SolicitacaoAcaoService>();
            services.AddScoped<IEscolaService, EscolaService>();
            services.AddHttpClient();
            services.AddAuth(configuration);
            
        }
    }
}
