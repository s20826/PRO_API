using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO;
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

        
        [HttpGet("{id}")]
        public IActionResult GetWeterynarzSpecjalizacjaList(int id)
        {
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + id + " nie ma specjalizacji zapisanych w systemie");
            }
            else
            {
                var results =
                from x in context.WeterynarzSpecjalizacjas
                join y in context.Specjalizacjas on x.IdSpecjalizacja equals y.IdSpecjalizacja into ps
                from p in ps
                where x.IdOsoba == id
                select new
                {
                    ID_specjalizacja = x.IdSpecjalizacja,
                    Nazwa = p.NazwaSpecjalizacji,
                    Opis = x.Opis
                };

                return Ok(results);
            }
        }

        [HttpGet("{id}/{idSpecjalizacja}")]
        public IActionResult GetWeterynarzSpecjalizacja(int id, int idSpecjalizacja)
        {
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + id + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }
            else
            {
                var results =
                from x in context.WeterynarzSpecjalizacjas
                join y in context.Specjalizacjas on x.IdSpecjalizacja equals y.IdSpecjalizacja into ps
                from p in ps
                where x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja
                select new
                {
                    Nazwa = p.NazwaSpecjalizacji,
                    Opis = x.Opis
                };

                return Ok(results);
            }
        }

        [HttpPost("{id}/{idSpecjalizacja}")]
        public IActionResult addWeterynarzSpecjalizacja(int id, int idSpecjalizacja, WeterynarzSpecjalizacjaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja).Any() == true)
            {
                return BadRequest("Już istnieje podany weterynarz o tej specjalizacji.");
            }

            context.WeterynarzSpecjalizacjas.Add(new WeterynarzSpecjalizacja
            {
                IdOsoba = id,
                IdSpecjalizacja =idSpecjalizacja,
                Opis = request.Opis
            });

            context.SaveChanges();

            return Ok("Pomyślnie dodano specjalizację weterynarzowi.");
        }

        [HttpPut("{id}/{idSpecjalizacja}")]
        public IActionResult updateWeterynarzSpecjalizacja(int id, int idSpecjalizacja, WeterynarzSpecjalizacjaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + id + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }

            var weterynarzSpecjalizacja = context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja).First();
            weterynarzSpecjalizacja.Opis = request.Opis;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{id}/{idSpecjalizacja}")]
        public IActionResult deleteWeterynarzSpecjalizacja(int id, int idSpecjalizacja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (context.WeterynarzSpecjalizacjas.Where(x => x.IdOsoba == id && x.IdSpecjalizacja == idSpecjalizacja).Any() != true)
            {
                return BadRequest("Weterynarz o ID = " + id + " nie ma specjalizacji o ID = " + idSpecjalizacja);
            }

            context.Remove(context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == idSpecjalizacja && x.IdOsoba == id).First());

            context.SaveChanges();

            return Ok("Pomyślnie usunięto specjalizację.");
        }
    }
}
