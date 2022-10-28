using Application.DTO.Requests;
using Application.GodzinaPracy.Commands;
using Application.GodzinaPracy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class GodzinyPracyController : ApiControllerBase
    {
        public GodzinyPracyController()
        {

        }

        //[Authorize(Roles = "admin")]
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
        [HttpPost("list/{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracyList(string ID_osoba, List<GodzinyPracyRequest> requests)
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
        [HttpPost("default/{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracyList(string ID_osoba)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateDefaultGodzinyPracyCommand
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
        public async Task<IActionResult> AddGodzinyPracy(string ID_osoba, GodzinyPracyRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = new List<GodzinyPracyRequest>() { request }
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteGodzinyPracy(string ID_osoba, int dzien)
        {
            try
            {
                await Mediator.Send(new DeleteGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    dzien = dzien
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
