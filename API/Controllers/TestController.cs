using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using System.Net.Mail;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        private readonly KlinikaContext context;
        public TestController(IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext)
        {
            configuration = config;
            hashids = ihashids;
            context = klinikaContext;
        }

        [HttpGet("hashid/{id}")]
        public async Task<IActionResult> GetHashedID(int id)        //test
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("password")]
        public async Task<IActionResult> GetHashedPassword()        //test
        {
            var salt = PasswordHelper.GenerateSalt();
            var myPassword = PasswordHelper.HashPassword(salt, "SecretPassword123", 20000);
            return Ok(new {
                PasswordHelper = myPassword,
                Length = myPassword.Length
            });
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()        //test
        {
            return Ok(context.Osobas.ToList());
        }
    }
}
