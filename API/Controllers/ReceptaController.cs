﻿using Application.Recepty.Commands;
using Application.Recepty.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ReceptaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_Klient}")]
        public async Task<IActionResult> GetReceptaKlientList(string ID_Klient, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaKlientQuery
                {
                    ID_klient = ID_Klient
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient")]
        [HttpGet("moje_recepty")]
        public async Task<IActionResult> GetReceptaKlientList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaKlientQuery
                {
                    ID_klient = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("details/{ID_Recepta}")]
        public async Task<IActionResult> GetReceptaDetails(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaDetailsQuery
                {
                    ID_recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPost("{ID_Recepta}")]
        public async Task<IActionResult> AddRecepta(string ID_Wizyta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateReceptaCommand
                {
                    ID_wizyta = ID_Wizyta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
