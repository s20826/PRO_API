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
    public class SpecjalizacjaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public SpecjalizacjaController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]
        public IActionResult GetSpecjalizacjaList()
        {
            var results =
                from x in context.Specjalizacjas
                select new
                {
                    ID_specjalizacja = x.IdSpecjalizacja,
                    Nazwa = x.NazwaSpecjalizacji
                };

            return Ok(results);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetSpecjalizacjaById(int id)
        {
            if (context.Znizkas.Where(x => x.IdZnizka == id).Any() != true)
            {
                return BadRequest("Nie ma specjalizacji o ID = " + id);
            } 
            else
            {
                var results =
                from x in context.Specjalizacjas
                select new
                {
                    ID_specjalizacja = x.IdSpecjalizacja,
                    Nazwa = x.NazwaSpecjalizacji
                };

                return Ok(results.First());
            }
        }


        [HttpPost]
        public IActionResult addSpecjalizacja(SpecjalizacjaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            context.Specjalizacjas.Add(new Specjalizacja
            {
                NazwaSpecjalizacji = request.NazwaSpecjalizacji
            });

            context.SaveChanges();

            return Ok("Dodano specjalizację: " + request.NazwaSpecjalizacji);
        }


        [HttpPut("{id}")]
        public IActionResult updateSpecjalizacja(int id, SpecjalizacjaRequest request)
        {
            if (!context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).Any())
            {
                return BadRequest("Nie ma specjalizacji o ID = " + id);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            var specjalizacja = context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).First();
            specjalizacja.NazwaSpecjalizacji = request.NazwaSpecjalizacji;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }


        [HttpDelete("{id}")]
        public IActionResult deleteSpecjalizacja(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }
            if (!context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).Any())
            {
                return BadRequest("Nie ma specjalizacji o ID = " + id);
            }

            context.Remove(context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id));

            context.SaveChanges();

            return Ok("Pomyślnie usunięto specjalizację.");
        }
    }
}
