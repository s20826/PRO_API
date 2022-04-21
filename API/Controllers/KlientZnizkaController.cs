using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.Models;
using System;
using System.Data;
using System.Linq;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlientZnizkaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public KlientZnizkaController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        

        [HttpGet("{ID_osoba}")]
        public IActionResult GetKlientZnizka(int ID_osoba)
        {
            if (context.KlientZnizkas.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Nie ma przyznanych znizek dla klienta o ID = " + ID_osoba);
            }
            else
            {
                var results =
                from x in context.Znizkas
                join y in context.KlientZnizkas on x.IdZnizka equals y.IdZnizka into ps
                from p in ps
                where p.IdOsoba == ID_osoba
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

        [HttpPost("{ID_osoba}/{idZnizka}")]
        public IActionResult addKlientZnizka(int ID_osoba, int idZnizka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            context.KlientZnizkas.Add(new KlientZnizka
            {
                IdOsoba = ID_osoba,
                IdZnizka = idZnizka,
                DataPrzyznania = DateTime.Now,
                CzyWykorzystana = false
            });

            context.SaveChanges();

            return Ok("Pomyślnie dodano zniżkę klientowi");
        }

        [HttpDelete("{ID_osoba}/{idZnizka}")]
        public IActionResult deleteKlientZnizka(int ID_osoba, int idZnizka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (!context.Znizkas.Where(x => x.IdZnizka == ID_osoba).Any())
            {
                return BadRequest("Nie ma zniżki o ID = " + ID_osoba);
            }

            context.Remove(context.KlientZnizkas.Where(x => x.IdZnizka == idZnizka && x.IdOsoba == ID_osoba).First());
            context.SaveChanges();

            return Ok("Pomyślnie usunięto zniżkę.");
        }
    }
}
