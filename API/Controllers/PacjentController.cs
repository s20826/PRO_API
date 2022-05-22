using Application.Commands.Pacjenci;
using Application.Commands.Pacjents;
using Application.DTO.Request;
using Application.Queries.Pacjent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacjentController : ApiControllerBase
    {
        public PacjentController()
        {
            
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetPacjentList()
        {
            return Ok(await Mediator.Send(new PacjentListQuery
            {

            }));
        }

        [Authorize]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientPacjentList(string ID_osoba)
        {
            if(isKlient() && !ID_osoba.Equals(GetUserId()))
            {
                return Unauthorized();
            }
            try
            {
                return Ok(await Mediator.Send(new PacjentKlientListQuery
                {
                    ID_osoba = ID_osoba
                }));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpGet("details/{ID_pacjent}")]
        public async Task<IActionResult> GetPacjentById(string ID_pacjent)
        {
            try
            {
                return Ok(await Mediator.Send(new PacjentDetailsQuery
                {
                    ID_pacjent = ID_pacjent
                }));
            } 
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddPacjent(PacjentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            try
            {
                return Ok(await Mediator.Send(new CreatePacjentCommand
                {
                    request = request
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_Pacjent}")]
        public async Task<IActionResult> UpdateKlient(string ID_Pacjent, PacjentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            try
            {
                await Mediator.Send(new UpdatePacjentCommand
                {
                    request = request,
                    ID_pacjent = ID_Pacjent
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_Pacjent}")]
        public async Task<IActionResult> DeleteKlient(string ID_Pacjent)
        {
            try
            {
                await Mediator.Send(new DeletePacjentCommand
                {
                    ID_Pacjent = ID_Pacjent
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
