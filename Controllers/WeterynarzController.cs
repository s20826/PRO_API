using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO;
using PRO_API.Models;
using System.Data;
using System.Data.SqlClient;
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

        [HttpGet("{ID_osoba}")]
        public IActionResult GetWeterynarzById(int ID_osoba)
        {
            if (context.Weterynarzs.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Nie ma weterynarza o ID = " + ID_osoba);
            } 
            else
            {
                var results =
                from x in context.Osobas
                join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == ID_osoba
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

        [HttpPost]
        public IActionResult addWeterynarz(WeterynarzRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", request.Imie);
            command.Parameters.AddWithValue("@nazwisko", request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", request.DataUrodzenia);
            command.Parameters.AddWithValue("@numerTel", request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@login", request.Login);
            command.Parameters.AddWithValue("@haslo", request.Haslo);
            command.Parameters.AddWithValue("@pensja", request.Pensja);
            command.Parameters.AddWithValue("@dataZatrudnienia", request.DataZatrudnienia);


            if (command.ExecuteNonQuery() == 2)
            {
                trans.Commit();
                return Ok("Dodano weterynarza " + request.Imie + " " + request.Nazwisko);
            }
            else
            {
                trans.Rollback();
                return BadRequest("Error, nie udało się dodać weterynarza");
            }
        }

        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateWeterynarz(int ID_osoba, KlientRequest request)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;
            konto.Login = request.Login;
            konto.Haslo = request.Haslo;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpPut("zatrudnienie/{ID_osoba}")]
        public IActionResult UpdateWeterynarzZatrudnienie(int ID_osoba, WeterynarzRequest request)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;
            konto.Login = request.Login;
            konto.Haslo = request.Haslo;
            
            var weterynarz = context.Weterynarzs.Where(x => x.IdOsoba == ID_osoba).First();
            weterynarz.Pensja = request.Pensja;
            weterynarz.DataZatrudnienia = request.DataZatrudnienia;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{ID_osoba}")]
        public IActionResult DeleteWeterynarz(int ID_osoba)
        {
            if (context.Weterynarzs.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            context.Remove(context.Weterynarzs.Where(x => x.IdOsoba == ID_osoba).First());
            context.Remove(context.Osobas.Where(x => x.IdOsoba == ID_osoba).First());
            context.SaveChanges();

            return Ok("Pomyślnie usunięto klienta.");
        }
    }
}
