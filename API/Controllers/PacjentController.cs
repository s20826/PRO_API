using Application.Queries.Pacjent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class PacjentController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        public PacjentController(IConfiguration config)
        {
            configuration = config;
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]       //admin, weterynarz
        public async Task<IActionResult> GetPacjentList()
        {
            return Ok(await Mediator.Send(new GetPacjentListQuery
            {

            }));
        }

        [Authorize]
        [HttpGet("{ID_osoba}")]     //klienta wyświetla swoje zwierzęta wchodząc w zakładkę na swoim koncie
        public async Task<IActionResult> GetKlientPacjentList(int ID_osoba)
        {
            return Ok(await Mediator.Send(new GetPacjentKlientListQuery
            {
                ID_osoba = ID_osoba
            }));
        }
        
        /*[HttpPost]  //weterynarz/admin
        public IActionResult addPacjent(PacjentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            context.Pacjents.Add(new Pacjent
            {
                IdOsoba = request.IdOsoba,
                Nazwa = request.Nazwa,
                Gatunek = request.Gatunek,
                Rasa = request.Rasa,
                Waga = request.Waga,
                Masc = request.Masc,
                DataUrodzenia = request.DataUrodzenia,
                Plec = request.Plec,
                Agresywne = request.Agresywne
            });

            context.SaveChanges();

            return Ok("Dodano pacjenta " + request.Nazwa);
        }
        
        [HttpPut("{ID_Pacjent}")]   //weterynarz/admin
        public IActionResult UpdateKlient(int ID_Pacjent, PacjentRequest request)
        {
            if (!context.Pacjents.Where(x => x.IdPacjent == ID_Pacjent).Any())
            {
                return BadRequest("Nie ma pacjenta o ID = " + ID_Pacjent);
            }
            var pacjent = context.Pacjents.Where(x => x.IdPacjent == ID_Pacjent).First();
            pacjent.Nazwa = request.Nazwa;
            pacjent.Gatunek = request.Gatunek;
            pacjent.Rasa = request.Rasa;
            pacjent.Waga = request.Waga;
            pacjent.Masc = request.Masc;
            pacjent.DataUrodzenia = request.DataUrodzenia;
            pacjent.Plec = request.Plec;
            pacjent.Agresywne = request.Agresywne;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{ID_Pacjent}")]    //weterynarz/admin
        public IActionResult DeleteKlient(int ID_Pacjent)
        {
            if (!context.Pacjents.Where(x => x.IdPacjent == ID_Pacjent).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_Pacjent);
            }
            context.Remove(context.Pacjents.Where(x => x.IdPacjent == ID_Pacjent).First());
            context.SaveChanges();

            return Ok("Pomyślnie usunięto pacjenta.");
        }*/
    }
}
