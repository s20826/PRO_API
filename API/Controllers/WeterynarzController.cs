using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeterynarzController : ControllerBase
    {
        /*private readonly IConfiguration configuration;
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
        public IActionResult addWeterynarz(WeterynarzPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Haslo,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            string saltBase64 = Convert.ToBase64String(salt);

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", request.Imie);
            command.Parameters.AddWithValue("@nazwisko", request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", request.DataUrodzenia);
            command.Parameters.AddWithValue("@numerTel", request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@login", request.Login);
            command.Parameters.AddWithValue("@haslo", hashed);
            command.Parameters.AddWithValue("@pensja", request.Pensja);
            command.Parameters.AddWithValue("@dataZatrudnienia", request.DataZatrudnienia);
            command.Parameters.AddWithValue("@salt", saltBase64);


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

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateWeterynarz(int ID_osoba, KlientPutRequest request)      //admin
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [Authorize(Roles = "admin")]
        [HttpPut("zatrudnienie/{ID_osoba}")]
        public IActionResult UpdateWeterynarzZatrudnienie(int ID_osoba, WeterynarzPutRequest request)       //admin
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;
            
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
        }*/
    }
}
