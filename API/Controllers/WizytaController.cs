using Application.Common.Exceptions;
using Application.Wizyty.Commands;
using Application.Wizyty.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytaController : ApiControllerBase
    {
        public WizytaController()
        {

        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetWizytaList()
        {
            return Ok(await Mediator.Send(new WizytaListQuery
            {

            }));
        }

        [Authorize]
        [HttpGet("moje_wizyty")]
        public async Task<IActionResult> GetWizytaKlient()
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new WizytaKlientQuery
                    {
                        ID_klient = GetUserId()
                    }));
                }
                return Ok(await Mediator.Send(new WizytaWeterynarzQuery
                {
                    ID_weterynarz = GetUserId()
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWizytaKlient(string ID_osoba)
        {
            try
            {
                return Ok(await Mediator.Send(new WizytaKlientQuery
                {
                    ID_klient = ID_osoba
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddWizyta(string ID_Harmonogram, string ID_Pacjent, string Notatka)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new UmowWizyteKlientCommand
                    {
                        ID_klient = GetUserId(),
                        ID_pacjent = ID_Pacjent,
                        ID_Harmonogram = ID_Harmonogram,
                        Notatka = Notatka
                    }));
                }

                return Ok(await Mediator.Send(new UmowWizyteKlientCommand
                {
                    //ID_klient = ID_klient,        //dodać do definicji metody
                    ID_pacjent = ID_Pacjent,
                    ID_Harmonogram = ID_Harmonogram,
                    Notatka = Notatka
                }));
            }
            catch (ConstraintException e)
            {
                return BadRequest(new
                {
                    message = e.Message,
                    value = e.ConstraintValue
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("{ID_wizyta}")]
        public async Task<IActionResult> DeleteWizyta(string ID_wizyta)    //klient albo weterynarz lub admin aunuluje wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteWizytaKlientCommand
                {
                    ID_wizyta = ID_wizyta
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{ID_wizyta}")]
        public async Task<IActionResult> DeleteWizytaByKlinika(string ID_wizyta)    //admin anuluje wizytę, status wizyty ustawiony jako anulowany przez klinike
        {
            try
            {
                /*return Ok(await Mediator.Send(new DeleteWizytaKlientCommand
                {
                    ID_wizyta = ID_wizyta
                }));*/

                throw new NotImplementedException();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
