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

        [HttpGet("weterynarz_specjalizacja/{id}")]
        public IActionResult GetWeterynarzSpecjalizacja(int id)
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
    }
}
