using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRO_API.DTO;
using PRO_API.DTO.Request;
using PRO_API.Models;
using System;
using System.Collections.Generic;
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
    public class AccountController : ControllerBase
    {
        /*private readonly IConfiguration configuration;
        private readonly KlinikaContext context;
        public AccountController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = context.Osobas.Where(x => x.NazwaUzytkownika == request.NazwaUzytkownika).FirstOrDefault();
            if (user == null)
            {
                return NotFound("Niepoprawne hasło lub login.");
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = PasswordHelper.HashPassword(salt, request.Haslo, int.Parse(configuration["PasswordIterations"]));

            if (passwordHash != currentHashedPassword)
            {
                return Unauthorized("Niepoprawne hasło lub login.");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", user.IdOsoba.ToString()),
                new Claim("login", user.NazwaUzytkownika)
            };

            if (user.Rola != null)
            {
                if (user.Rola.Equals("A"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                if (user.Rola.Equals("W"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "weterynarz"));
                }
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "klient"));
            }


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

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
        public IActionResult GetToken(Guid refreshToken)
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

        
        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateAccountCredentials(int ID_osoba, AccountCredentialsRequest request)
        {
            if (!context.Osobas.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = PasswordHelper.HashPassword(salt, request.currentHaslo, int.Parse(configuration["PasswordIterations"]));

            if (passwordHash != currentHashedPassword)
            {
                return BadRequest("Niepoprawne hasło.");
            }

            string hashed = PasswordHelper.HashPassword(salt, request.newHaslo, int.Parse(configuration["PasswordIterations"]));

            user.NumerTelefonu = request.NumerTelefonu;
            user.Email = request.Email;
            user.Haslo = hashed;

            context.SaveChanges();
            return Ok("Pomyślnie zaktuzalizowano dane.");
        }*/
    }
}
