using Application.Interfaces;
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
            services.AddScoped<ILekRepository, LekService>();
            services.AddScoped<IKontoRepository, KontoService>();
            services.AddScoped<IKlientRepository, KlientService>();
            services.AddScoped<IWeterynarzRepository, WeterynarzService>();

            return services;
        }
    }
}
