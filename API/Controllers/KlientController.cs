using Application.Commands.Klient;
using Application.DTO;
using Application.Queries.Klient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    public class KlientController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        public KlientController(IConfiguration config)
        {
            configuration = config;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetKlientList()
        {
            return Ok(await Mediator.Send(new GetKlientListQuery
            {

            }));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientById(int ID_osoba)
        {
            return Ok(await Mediator.Send(new GetKlientQuery
            {
                ID_osoba = ID_osoba
            }));
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddKlient(KlientCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Mediator.Send(new CreateKlientCommand
                {
                    request = request
                }));
            } 
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message
                });
            }
        }


        [Authorize(Roles = "admin,klient")]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteKlient(int ID_osoba)
        {
            await Mediator.Send(new DeleteKlientCommand
            {
                ID_osoba = ID_osoba
            });

            return NoContent();
        }


        /*[Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateKlient(int ID_osoba, KlientPostRequest request)      //admin
        {
            if (!context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
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
            konto.NazwaUzytkownika = request.NazwaUzytkownika;
            konto.Haslo = request.Haslo;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }*/
    }
}
