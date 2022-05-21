using Application.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenService>();
            services.AddScoped<IPasswordRepository, PasswordService>();
            services.AddScoped<IHash, HashService>();

            services.AddScoped<IKlinikaContext, KlinikaContext>();

            return services;
        }
    }
}
