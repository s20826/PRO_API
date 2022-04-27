using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class KontoService : IKontoRepository
    {
        private readonly KlinikaContext context;
        private readonly IConfiguration configuration;
        public KontoService(KlinikaContext klinikaContext, IConfiguration config)
        {
            context = klinikaContext;
            configuration = config;
        }

        public async Task<GetKontoResponse> GetKonto(int ID_osoba)
        {
            return context.Osobas.Where(x => x.IdOsoba == ID_osoba).Select(x => new GetKontoResponse()
            {
                Imie = x.Imie,
                Nazwisko = x.Nazwisko,
                NumerTelefonu = x.NumerTelefonu,
                Email = x.Email
            }).FirstOrDefault();
        }

        public async Task<LoginTokens> Login(LoginRequest request)
        {
            var user = context.Osobas.Where(x => x.NazwaUzytkownika == request.NazwaUzytkownika).FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException("Niepoprawne hasło lub login.");
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = PasswordHelper.HashPassword(salt, request.Haslo, int.Parse(configuration["PasswordIterations"]));

            if (passwordHash != currentHashedPassword)
            {
                throw new UserNotAuthorizedException("Niepoprawne hasło lub login.");
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
            await context.SaveChangesAsync();

            return new LoginTokens()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        public async Task<string> GetToken(Guid refreshToken)
        {
            if (refreshToken.ToString().Length == 0)
            {
                throw new NotFoundException("Nie znaleziono Refresh Token");
            }
            var user = context.Osobas.SingleOrDefault(x => x.RefreshToken == refreshToken.ToString());
            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono Refresh Token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {
                throw new UserNotAuthorizedException("Refresh Token wygasł");
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
                issuer: "http://loclahost:5001",
                audience: "http://loclahost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> UpdateKontoCredentials(int ID_osoba, KontoUpdateRequest request)
        {
            var user = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            if(user == null)
            {
                throw new NotFoundException();
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = PasswordHelper.HashPassword(salt, request.currentHaslo, int.Parse(configuration["PasswordIterations"]));

            if (passwordHash != currentHashedPassword)
            {
                throw new UserNotAuthorizedException("Niepoprawne hasło.");
            }

            string hashed = PasswordHelper.HashPassword(salt, request.newHaslo, int.Parse(configuration["PasswordIterations"]));

            user.NumerTelefonu = request.NumerTelefonu;
            user.Email = request.Email;
            user.Haslo = hashed;

            return await context.SaveChangesAsync();
        }
    }
}
