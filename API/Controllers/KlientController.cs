using Application.Commands.Klient;
using Application.DTO;
using Application.Queries.Klient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlientController : ApiControllerBase
    {
        public KlientController()
        {

        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetKlientList()
        {
            return Ok(await Mediator.Send(new KlientListQuery
            {

            }));
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientById(string ID_osoba)
        {
            try
            {
                return Ok(await Mediator.Send(new KlientQuery
                {
                    ID_osoba = ID_osoba
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> DeleteKlient(string ID_osoba)
        {
            try
            {
                await Mediator.Send(new DeleteKlientCommand
                {
                    ID_osoba = ID_osoba
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }
            
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
