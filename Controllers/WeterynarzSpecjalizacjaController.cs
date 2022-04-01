using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO;
using PRO_API.DTO.Request;
using PRO_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeterynarzSpecjalizacjaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public WeterynarzSpecjalizacjaController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        
        [HttpGet("{ID_osoba}")]
        public IActionResult GetWeterynarzSpecjalizacjaList(int ID_osoba)
        {
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + ID_osoba + " nie ma specjalizacji zapisanych w systemie");
            }
            else
            {
                var results =
                from x in context.WeterynarzSpecjalizacjas
                join y in context.Specjalizacjas on x.IdSpecjalizacja equals y.IdSpecjalizacja into ps
                from p in ps
                where x.IdOsoba == ID_osoba
                select new
                {
                    ID_specjalizacja = x.IdSpecjalizacja,
                    Nazwa = p.NazwaSpecjalizacji,
                    Opis = x.Opis
                };

                return Ok(results);
            }
        }

        [HttpGet("{ID_osoba}/{idSpecjalizacja}")]
        public IActionResult GetWeterynarzSpecjalizacja(int ID_osoba, int idSpecjalizacja)
        {
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + ID_osoba + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }
            else
            {
                var results =
                from x in context.WeterynarzSpecjalizacjas
                join y in context.Specjalizacjas on x.IdSpecjalizacja equals y.IdSpecjalizacja into ps
                from p in ps
                where x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja
                select new
                {
                    Nazwa = p.NazwaSpecjalizacji,
                    Opis = x.Opis
                };

                return Ok(results);
            }
        }

        [HttpPost("{ID_osoba}/{idSpecjalizacja}")]
        public IActionResult addWeterynarzSpecjalizacja(int ID_osoba, int idSpecjalizacja, WeterynarzSpecjalizacjaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja).Any() == true)
            {
                return BadRequest("Już istnieje podany weterynarz o tej specjalizacji.");
            }

            context.WeterynarzSpecjalizacjas.Add(new WeterynarzSpecjalizacja
            {
                IdOsoba = ID_osoba,
                IdSpecjalizacja =idSpecjalizacja,
                Opis = request.Opis
            });

            context.SaveChanges();

            return Ok("Pomyślnie dodano specjalizację weterynarzowi.");
        }

        [HttpPut("{ID_osoba}/{idSpecjalizacja}")]
        public IActionResult updateWeterynarzSpecjalizacja(int ID_osoba, int idSpecjalizacja, WeterynarzSpecjalizacjaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + ID_osoba + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }

            var weterynarzSpecjalizacja = context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja).First();
            weterynarzSpecjalizacja.Opis = request.Opis;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{ID_osoba}/{idSpecjalizacja}")]
        public IActionResult deleteWeterynarzSpecjalizacja(int ID_osoba, int idSpecjalizacja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == ID_osoba && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + ID_osoba + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }

            context.Remove(context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == idSpecjalizacja && x.IdOsoba == ID_osoba).First());

            context.SaveChanges();

            return Ok("Pomyślnie usunięto specjalizację.");
        }
    }
}
