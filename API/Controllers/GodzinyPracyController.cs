using Application.Commands.GodzinyPracy;
using Application.DTO.Requests;
using Application.Queries.GodzinyPracy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GodzinyPracyController : ApiControllerBase
    {
        public GodzinyPracyController()
        {

        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetGodzinyPracy(string ID_osoba)
        {
            try
            {
                return Ok(await Mediator.Send(new GodzinyPracyQuery
                {
                    ID_osoba = ID_osoba
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracy(string ID_osoba, List<GodzinyPracyRequest> requests)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = requests
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateGodzinyPracy(string ID_osoba, List<GodzinyPracyRequest> requests)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = requests
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
