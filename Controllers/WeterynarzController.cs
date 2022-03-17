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
    public class WeterynarzController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public WeterynarzController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]
        public IActionResult GetWeterynarzList()
        {
            var results =
                from x in context.Osobas
                join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                select new
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZatrudnienia
                };


            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetWeterynarzById(int id)
        {
            if (context.Weterynarzs.Where(x => x.IdOsoba == id).Any() != true)
            {
                return BadRequest("Nie ma weterynarza o ID = " + id);
            } 
            else
            {
                var results =
                from x in context.Osobas
                join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == id
                select new
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZatrudnienia,
                    Pensja = p.Pensja
                };

                return Ok(results.First());
            }
        }
    }
}
