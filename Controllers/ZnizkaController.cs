using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.Models;
using System.Data;
using System.Linq;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZnizkaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public ZnizkaController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]
        public IActionResult GetZnizkaList()
        {
            var results =
                from x in context.Znizkas
                select new
                {
                    ID_znizka = x.IdZnizka,
                    Nazwa = x.NazwaZnizki,
                    Procent = x.ProcentZnizki
                };

            return Ok(results);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetZnizkaById(int id)
        {
            if (context.Znizkas.Where(x => x.IdZnizka == id).Any() != true)
            {
                return BadRequest("Nie ma znizki o ID = " + id);
            } 
            else
            {
                var results =
                from x in context.Znizkas
                where x.IdZnizka == id
                select new
                {
                    Nazwa = x.NazwaZnizki,
                    Procent = x.ProcentZnizki
                };

                return Ok(results.First());
            }
        }

        [HttpGet("klient_znizka/{id}")]
        public IActionResult GetKlientZnizka(int id)
        {
            if (context.KlientZnizkas.Where(x => x.IdOsoba == id).Any() != true)
            {
                return BadRequest("Nie ma przyznanych znizek dla klienta o ID = " + id);
            }
            else
            {
                var results =
                from x in context.Znizkas
                join y in context.KlientZnizkas on x.IdZnizka equals y.IdZnizka into ps
                from p in ps
                where p.IdOsoba == id
                select new
                {
                    Nazwa = x.NazwaZnizki,
                    Procent = x.ProcentZnizki,
                    Data_przyznania = p.DataPrzyznania,
                    Czy_wykorzystana = p.CzyWykorzystana
                };

                return Ok(results);
            }
        }
    }
}
