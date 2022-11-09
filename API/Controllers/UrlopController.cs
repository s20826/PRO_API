using Application.DTO.Requests;
using Application.Urlopy.Commands;
using Application.Urlopy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class UrlopController : ApiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUrlopList()
        {
            return Ok(await Mediator.Send(new UrlopListQuery
            {

            }));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzUrlopList(string ID_weterynarz)
        {
            try
            {
                return Ok(await Mediator.Send(new UrlopWeterynarzQuery
                {
                    ID_weterynarz = ID_weterynarz
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("details/{ID_urlop}")]
        public async Task<IActionResult> GetUrlopDetails(string ID_urlop)
        {
            try
            {
                return Ok(await Mediator.Send(new UrlopDetailsQuery
                {
                    ID_urlop = ID_urlop
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddUrlop(UrlopRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateUrlopCommand
                {
                    request = request
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_urlop}")]
        public async Task<IActionResult> UpdateUrlop(string ID_urlop, UrlopRequest request)
        {
            try
            {
                await Mediator.Send(new UpdateUrlopCommand
                {
                    ID_urlop = ID_urlop,
                    request = request
                });
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_urlop}")]
        public async Task<IActionResult> DeleteUrlop(string ID_urlop)
        {
            try
            {
                await Mediator.Send(new DeleteUrlopCommand
                {
                    ID_urlop = ID_urlop
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