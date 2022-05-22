using Microsoft.AspNetCore.Mvc;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytaController : ControllerBase
    {
        public WizytaController()
        {

        }

        /*[HttpGet]   //admin
        public IActionResult GetWizytaList()
        {
            var results = context.Wizyta;
            return Ok(results);
        }

        [HttpGet("{ID_wizyta}")]   //admin
        public IActionResult GetWizytaById(int ID_wizyta)
        {
            if (context.Wizyta.Where(x => x.IdWizyta == ID_wizyta).Any() != true)
            {
                return BadRequest("Nie ma wizyty o ID = " + ID_wizyta);
            }
            var result = context.Wizyta.Where(x => x.IdWizyta == ID_wizyta).FirstOrDefault();
            return Ok(result);
        }

        [HttpGet("/weterynarz/{ID_osoba}")]   //weterynarz
        public IActionResult GetWizytaListByWeterynarz(int ID_osoba)
        {
            if (context.Wizyta.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                return BadRequest("Nie masz przypisanych wizyt.");
            }
            var result = context.Wizyta.Where(x => x.IdOsoba == ID_osoba);
            return Ok(result);
        }

        [HttpGet("/pacjent/{ID_pacjent}")]   //wizyty danego pacjenta
        public IActionResult GetPacjentWizytaList(int ID_pacjent)
        {
            if (context.Wizyta.Where(x => x.IdPacjent == ID_pacjent).Any() != true)
            {
                return BadRequest("Nie ma pacjenta o ID = " + ID_pacjent);
            }
            var result = context.Wizyta.Where(x => x.IdPacjent == ID_pacjent);
            return Ok(result);
        }

        [HttpGet("/klient/{ID_osoba}")]   //wizyty danego klienta
        public IActionResult GetKlientWizytaList(int ID_osoba)
        {
            var results = context.Wizyta.Where(z => z.IdPacjentNavigation.IdOsoba == ID_osoba);
            if (!results.Any())
            {
                return BadRequest("Nie masz zapisanych pacjentów");
            }
            return Ok(results);
        }*/
    }
}
