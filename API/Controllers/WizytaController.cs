using Application.Queries.Wizyta;
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return NotFound();
            }
        }

        /*[Authorize(Roles = "klient,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddWizyta()    //klient albo weterynarz umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }*/
    }
}
