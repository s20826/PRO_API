using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRO_API.DTO;
using PRO_API.DTO.Request;
using PRO_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlientController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;
        public KlientController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetKlientList()
        {
            var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                select new
                {
                    ID_osoba = x.IdOsoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZalozeniaKonta
                };

            return Ok(results);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public IActionResult GetKlientById(int ID_osoba)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Nie ma klienta o ID = " + ID_osoba);
            } 
            else
            {
                var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == ID_osoba
                select new
                {
                    ID_osoba = ID_osoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZalozeniaKonta
                };

                return Ok(results.First());
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult addKlient(KlientRequest request)
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

            var query = "exec DodajKlienta @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", request.Imie);
            command.Parameters.AddWithValue("@nazwisko", request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", request.DataUrodzenia);
            command.Parameters.AddWithValue("@numerTel", request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@login", request.Login);
            command.Parameters.AddWithValue("@haslo", hashed);
            command.Parameters.AddWithValue("@salt", saltBase64);

            
            if (command.ExecuteNonQuery() == 2)
            {
                trans.Commit();
                return Ok("Dodano klienta " + request.Imie + " " + request.Nazwisko);
            }
            else
            {
                trans.Rollback();
                return BadRequest("Error, nie udało się dodać klienta ");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateKlient(int ID_osoba, KlientRequest request)
        {
            if (!context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_osoba}")]
        public IActionResult DeleteKlient(int ID_osoba)
        {
            if (!context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            context.Remove(context.Klients.Where(x => x.IdOsoba == ID_osoba).First());
            context.Remove(context.Osobas.Where(x => x.IdOsoba == ID_osoba).First());
            context.SaveChanges();

            return Ok("Pomyślnie usunięto klienta.");
        }
    }
}
