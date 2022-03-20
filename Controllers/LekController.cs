using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class LekController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;
        public LekController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]
        public IActionResult GetLekList()
        {
            var query = "Select l.ID_lek, l.Nazwa, SUM(ilosc) as Ilosc, l.Jednostka_Miary from Lek l, Lek_W_Magazynie m " +
                "where l.ID_lek = m.ID_lek AND Data_waznosci > GETDATE()" +
                "group by Nazwa, Jednostka_Miary, l.ID_lek";

            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            table.Load(reader);

            reader.Close();
            connection.Close();

            return Ok(table);
        }

        [HttpGet("{ID_lek}")]
        public IActionResult GetLekById(int ID_lek)
        {
            if (context.Leks.Where(x => x.IdLek == ID_lek).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }
            else
            {
                var results =
                from x in context.Leks
                join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
                from p in ps
                where x.IdLek == ID_lek
                select new
                {
                    IdStanLeku = p.IdStanLeku,
                    Nazwa = x.Nazwa,
                    Jednostka_Miary = x.JednostkaMiary,
                    Ilosc = p.Ilosc,
                    Data_Waznosci = p.DataWaznosci
                };

                return Ok(results);
            }
        }

        [HttpGet("{ID_lek}/{ID_stan_leku}")]
        public IActionResult GetLekWMagazynieById(int ID_lek, int ID_stan_leku)
        {
            if (context.Leks.Where(x => x.IdLek == ID_lek).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }
            else if(context.LekWMagazynies.Where(x => x.IdStanLeku == ID_stan_leku).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek + "w magazynie");
            }
            else
            {
                var results =
                from x in context.Leks
                join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
                from p in ps
                where x.IdLek == ID_lek && p.IdStanLeku == ID_stan_leku
                select new
                {
                    IdStanLeku = p.IdStanLeku,
                    Nazwa = x.Nazwa,
                    Jednostka_Miary = x.JednostkaMiary,
                    Ilosc = p.Ilosc,
                    Data_Waznosci = p.DataWaznosci
                };

                return Ok(results);
            }
        }

        /*[HttpPost]
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
        }*/
    }
}