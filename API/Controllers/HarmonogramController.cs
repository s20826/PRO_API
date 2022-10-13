using Application.Harmonogramy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarmonogramController : ApiControllerBase
    {
        [Authorize(Roles = "klient,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetHarmonogram(DateTime date)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new HarmonogramKlientQuery
                    {
                        Date = date
                    }));
                }

                return Ok(await Mediator.Send(new HarmonogramWeterynarzQuery
                {
                    Date = date
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlinikaHarmonogram(string ID_osoba, DateTime startDate, DateTime endDate)
        {
            try
            {
                return Ok(await Mediator.Send(new HarmonogramAdminQuery
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
    }
}
