using Application.Interfaces;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PasswordService : IPasswordRepository
    {
        private readonly IConfiguration configuration;

        public PasswordService(IConfiguration config)
        {
            configuration = config;
        }
        public async Task<string> GetHashed(byte[] salt, string plainPassword)
        {
            return PasswordHelper.HashPassword(salt, plainPassword, int.Parse(configuration["PasswordIterations"]));
        }
    }
}
