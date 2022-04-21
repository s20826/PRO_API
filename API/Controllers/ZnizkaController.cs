using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO.Request;
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

        [HttpGet("{id}")]
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

        [HttpPost]
        public IActionResult addZnizka(ZnizkaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            context.Znizkas.Add(new Znizka
            {
                NazwaZnizki = request.NazwaZnizki,
                ProcentZnizki = request.ProcentZnizki,
                DoKiedy = request.DoKiedy
            });

            context.SaveChanges();

            return Ok("Dodano zniżkę: " + request.NazwaZnizki);
        }

        [HttpPut("{id}")]
        public IActionResult updateZnizka(int id, ZnizkaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (!context.Znizkas.Where(x => x.IdZnizka == id).Any())
            {
                return BadRequest("Nie ma zniżki o ID = " + id);
            }

            var znizka = context.Znizkas.Where(x => x.IdZnizka == id).First();
            znizka.NazwaZnizki = request.NazwaZnizki;
            znizka.ProcentZnizki = request.ProcentZnizki;
            znizka.DoKiedy = request.DoKiedy;
            
            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{id}")]
        public IActionResult deleteZnizka(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (!context.Znizkas.Where(x => x.IdZnizka == id).Any())
            {
                return BadRequest("Nie ma zniżki o ID = " + id);
            }

            context.Remove(context.Znizkas.Where(x => x.IdZnizka == id).First());
            context.SaveChanges();

            return Ok("Pomyślnie usunięto zniżkę.");
        }
    }
}
