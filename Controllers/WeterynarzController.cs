using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.Models;
using System.Data;
using System.Linq;

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
                    ID_Osoba = x.IdOsoba,
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
                    Data_Urodzenia = x.DataUrodzenia,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zatrudnienia = p.DataZatrudnienia,
                    Pensja = p.Pensja
                };

                return Ok(results.First());
            }
        }
    }
}
