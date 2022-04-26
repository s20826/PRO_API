using Application.Commands.Konto;
using Application.DTO.Request;
using Application.Queries.Konto;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    public class KontoController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        public KontoController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKonto(int ID_osoba)
        {
            return Ok(await Mediator.Send(new GetKontoQuery
            {
                ID_osoba = ID_osoba
            }));
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Mediator.Send(new LoginCommand
                {
                    request = request
                }));
            } catch (Exception e)
            {
                switch (e)
                {
                    case NotFoundException:
                        return NotFound(e.Message);
                    case UserNotAuthorizedException:
                        return Unauthorized(e.Message);
                    default:
                        return BadRequest();
                }
            }
        }

        /*[AllowAnonymous]
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
