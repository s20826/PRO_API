using Application.DTO;
using Application.Weterynarze.Commands;
using Application.Weterynarze.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeterynarzController : ApiControllerBase
    {
        public WeterynarzController()
        {
            
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetWeterynarzList()
        {
            return Ok(await Mediator.Send(new WeterynarzListQuery
            {
                
            }));
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWeterynarzById(string ID_osoba)
        {
            return Ok(await Mediator.Send(new WeterynarzQuery
            {
                ID_osoba = ID_osoba
            }));
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddWeterynarz(WeterynarzCreateRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateWeterynarzCommand
                {
                    request = request
                }));
            }
            catch (Exception e)
            {
                return NotFound(new
                {
                    message = e.Message
                });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateWeterynarzZatrudnienie(string ID_osoba, WeterynarzUpdateRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateWeterynarzCommand
                {
                    ID_osoba = ID_osoba,
                    request = request
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteWeterynarz(string ID_osoba)
        {
            try
            {
                await Mediator.Send(new DeleteWeterynarzCommand
                {
                    ID_osoba = ID_osoba
                });
            }
            catch(Exception)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
