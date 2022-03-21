using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO;
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
    public class PacjentController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public PacjentController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]   //admin
        public IActionResult GetPacjentList()
        {
            var results = context.Pacjents;

            return Ok(results);
        }

        [HttpGet("{ID_osoba}")] //klienta wyświetla swoje zwierzęta wchodząc w zakładkę na swoim koncie
        public IActionResult GetKlientPacjentList(int ID_osoba)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Nie ma klienta o ID = " + ID_osoba);
            } 
            else
            {
                var results =
                from x in context.Pacjents
                where x.IdOsoba == ID_osoba
                select new
                {
                    ID_pacjent = x.IdPacjent,
                    Nazwa = x.Nazwa,
                    Gatunek = x.Gatunek,
                    Rasa = x.Rasa,
                    Waga = x.Waga,
                    Masc = x.Masc,
                    Data_urodzenia = x.DataUrodzenia,
                    Plec = x.Plec
                };

                return Ok(results);
            }
        }
        
        [HttpPost]  //weterynarz
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

            return Ok("Dodano pacjenta");
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
        }
    }
}
