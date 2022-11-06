using Application.Harmonogramy.Commands;
using Application.Harmonogramy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class HarmonogramController : ApiControllerBase
    {
        //ustawia harmonogramy na podany dzień dla wszystkich weterynarzy według ich godzin pracy
        [HttpPost("day")]
        public async Task<IActionResult> AddHarmonogramsForADay(DateTime date)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateHarmonogramDefaultCommand
                {
                    Data = date
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //ustawia harmonogramy na podany dzień dla wszystkich weterynarzy według ich godzin pracy
        [HttpPost("day/{ID_weterynarz}")]
        public async Task<IActionResult> AddWeterynarzHarmonogramForADay(DateTime date, string ID_weterynarz)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateHarmonogramByIDCommand
                {
                    Data = date,
                    ID_weterynarz = ID_weterynarz
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //usuwa harmonogramy na podany dzień wszystkim weterynarzom
        [HttpDelete("day")]
        public async Task<IActionResult> DeleteHarmonogramsForADay(DateTime date)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteHarmonogramCommand
                {
                    Data = date
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //klient umawia wizytę albo pracownik kliniki umówia wizytę na prośbę klienta
        //[Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet]
        public async Task<IActionResult> GetHarmonogram(DateTime date)      
        {
            try
            {
                return Ok(await Mediator.Send(new HarmonogramKlientQuery
                {
                    Date = date
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //admin wyświetla harmonogram weterynarza
        [Authorize(Roles = "admin")]
        [HttpGet("klinika/{ID_osoba}")]
        public async Task<IActionResult> GetKlinikaAdminHarmonogram(string ID_osoba, DateTime startDate, DateTime endDate)
        {
            try
            {
                return Ok(await Mediator.Send(new HarmonogramAdminByIDQuery
                {
                    ID_osoba = ID_osoba,
                    StartDate = startDate,
                    EndDate = endDate
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "weterynarz,admin")]
        [HttpGet("klinika")]
        public async Task<IActionResult> GetKlinikaHarmonogram(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (isWeterynarz())
                {
                    return Ok(await Mediator.Send(new HarmonogramWeterynarzQuery
                    {
                        ID_osoba = GetUserId(),
                        StartDate = startDate,
                        EndDate = endDate
                    }));
                }

                return Ok(await Mediator.Send(new HarmonogramAdminQuery
                {
                    StartDate = startDate,
                    EndDate = endDate
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}