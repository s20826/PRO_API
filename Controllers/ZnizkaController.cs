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
            if (context.KlientZnizkas.Where(x => x.KlientIdOsoba == id).Any() != true)
            {
                return BadRequest("Nie ma przyznanych znizek dla klienta o ID = " + id);
            }
            else
            {
                var results =
                from x in context.Znizkas
                join y in context.KlientZnizkas on x.IdZnizka equals y.ZnizkaIdZnizka into ps
                from p in ps
                where x.IdZnizka == id
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
