using Application.Common.Exceptions;
using Application.DTO.Requests;
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

        [Authorize(Roles = "klient,weterynarz")]
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
        [HttpGet("{ID_klient}")]
        public async Task<IActionResult> GetWizytaKlient(string ID_klient)
        {
            try
            {
                return Ok(await Mediator.Send(new WizytaAdminQuery
                {
                    ID_klient = ID_klient
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize]
        [HttpGet("pacjent/{ID_Pacjent}")]
        public async Task<IActionResult> GetWizytaPacjent(string ID_Pacjent)
        {
            try
            {
                return Ok(await Mediator.Send(new WizytaPacjentQuery
                {
                    ID_Pacjent = ID_Pacjent
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("details/{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaDetails(string ID_wizyta)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new WizytaDetailsKlientQuery
                    {
                        ID_klient = GetUserId(),
                        ID_wizyta = ID_wizyta
                    }));
                }

                return Ok(await Mediator.Send(new WizytaDetailsAdminQuery
                {
                    ID_wizyta = ID_wizyta
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddWizyta(UmowWizyteRequest request)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new UmowWizyteKlientCommand
                    {
                        ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_Harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }));
                }

                return Ok(await Mediator.Send(new UmowWizyteKlientCommand
                {
                    ID_klient = request.ID_Klient,
                    ID_pacjent = request.ID_Pacjent,
                    ID_Harmonogram = request.ID_Harmonogram,
                    Notatka = request.Notatka
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
            catch (Exception e)
            {
                return BadRequest(new
                {
                    massage = e.Message
                });
            }
        }

        [Authorize]
        [HttpPut("przeloz/{ID_wizyta}")]
        public async Task<IActionResult> UpdateWizytaData(UmowWizyteRequest request, string ID_wizyta)    //klient albo weterynarz lub admin zmienia termin wizyty dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new PrzelozWizyteCommand
                    {
                        ID_wizyta = ID_wizyta,
                        ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_Harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }));
                }

                return Ok(await Mediator.Send(new PrzelozWizyteCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_klient = request.ID_Klient,
                    ID_pacjent = request.ID_Pacjent,
                    ID_Harmonogram = request.ID_Harmonogram,
                    Notatka = request.Notatka
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
            catch (Exception e)
            {
                return BadRequest(new
                {
                    massage = e.Message
                });
            }
        }

        [Authorize(Roles = "weterynarz")]
        [HttpPut("{ID_wizyta}")]
        public async Task<IActionResult> UpdateWizytaInfo(WizytaInfoUpdateRequest request, string ID_wizyta)    //weterynarz zmienia informacje o wizycie (opis, status, dodaje wykonane usługi)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateWizytaInfoCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_weterynarz = GetUserId(),
                    request = request
                }));
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    massage = e.Message
                });
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
                return Ok(await Mediator.Send(new DeleteWizytaAdminCommand
                {
                    ID_wizyta = ID_wizyta
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpDelete("system")]
        public async Task<IActionResult> DeleteWizytaBySystem()    //system usuwa wszystkie anulowane wizyty
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteWizytaSystemCommand
                {

                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}