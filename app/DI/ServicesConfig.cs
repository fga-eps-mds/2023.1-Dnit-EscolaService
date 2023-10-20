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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var configuracaoAutenticaco = configuration.GetSection("Autenticacao");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuracaoAutenticaco["Issuer"],
                    ValidAudience = configuracaoAutenticaco["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuracaoAutenticaco["Key"]!)),
                    ValidateIssuer = bool.Parse(configuracaoAutenticaco["ValidateIssuer"]!),
                    ValidateAudience = bool.Parse(configuracaoAutenticaco["ValidateAudience"]!),
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = bool.Parse(configuracaoAutenticaco["ValidateIssuerSigningKey"]!)
                };
            });

            services.AddAuthorization();
        }
    }
}
