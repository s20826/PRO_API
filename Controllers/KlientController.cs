using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRO_API.DTO;
using PRO_API.DTO.Request;
using PRO_API.Models;
using PRO_API.Other;
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
        private readonly TokensGenerator tokensGenerator;
        public KlientController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
            tokensGenerator = new TokensGenerator(config);
        }

        [Authorize]
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

        [Authorize]
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
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = context.Osobas.Where(x => x.Login == request.Login).FirstOrDefault();
            if (user == null)
            {
                return NotFound("Niepoprawne hasło lub login.");
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);

            string currentHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Haslo,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (passwordHash != currentHashedPassword)
            {
                return Unauthorized("Niepoprawne hasło lub login.");
            }

            Claim[] userclaim = new[]
            {
                new Claim(ClaimTypes.Name, user.IdOsoba.ToString()),
                //new Claim(ClaimTypes.Role, "admin")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
                );


            //var token = tokensGenerator.GenerateAccessToken();

             
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(3);
            context.SaveChanges();


            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public IActionResult getToken(Guid refreshToken)
        {
            var user = context.Osobas.SingleOrDefault(x => x.RefreshToken == refreshToken.ToString());
            if (user == null)
            {
                return NotFound("Nie znaleziono Refresh Token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {
                return BadRequest("Refresh Token wygasł");
            }

            Claim[] userclaim = new[]
             {
                new Claim(ClaimTypes.Name, user.IdOsoba.ToString()),
                //new Claim(ClaimTypes.Role, "admin")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://loclahost:5001",
                audience: "http://loclahost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

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

            var query = "exec P1 @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @salt";
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

        /*[HttpGet]
        public IActionResult GetKlienci2()
        {
            var query = "Select Imie, Nazwisko, Numer_telefonu, Email, Data_zalozenia_konta from Klient k, Osoba o Where k.ID_osoba = o.ID_osoba";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<KlientResponse> klientResponses = new List<KlientResponse>();
            while (reader.Read())
            {
                klientResponses.Add(new KlientResponse
                {
                    Imie = reader["Imie"].ToString(),
                    Nazwisko = reader["Nazwisko"].ToString(),
                    NumerTelefonu = reader["Numer_telefonu"].ToString(),
                    Email = reader["Email"].ToString(),
                    DataZalozeniaKonta = (DateTime)reader["Data_zalozenia_konta"]
                });
            }
            reader.Close();
            connection.Close();
        }*/
    }
}
