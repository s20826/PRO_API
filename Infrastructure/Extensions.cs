using Application.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenService>();
            services.AddScoped<IPasswordRepository, PasswordService>();
            services.AddScoped<IHash, HashService>();
            services.AddScoped<IWizytaRepository, WizytaService>();
            services.AddScoped<ILoginRepository, LoginService>();
            services.AddScoped<IHarmonogramRepository, HarmonogramService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IKlinikaContext, KlinikaContext>();

            return services;
        }
    }
}
