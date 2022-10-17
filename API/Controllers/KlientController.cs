using Application.DTO;
using Application.Klienci.Commands;
using Application.Klienci.Queries;
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


        [Authorize(Roles = "admin,weterynarz")]
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
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddKlient(KlientCreateRequest request)
        {
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
            catch (Exception)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
