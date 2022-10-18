using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;

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
        public async Task<IActionResult> GetHashedID(int id)
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            return Ok(context.Osobas.ToList());
        }

/*        [HttpGet("sdfsdfsdfd")]
        public async Task<IActionResult> dfsdfsdfsd(int id)
        {
            for(int i = 0; i < 5; i++)
            {
                context.GodzinyPracies.Add(new Domain.Models.GodzinyPracy
                {
                    DzienTygodnia = i,
                    GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                    GodzinaZakonczenia = new TimeSpan(17,0,0),
                    IdOsoba = id
                });
            }

            return Ok(context.SaveChanges());
        }*/
    }
}
