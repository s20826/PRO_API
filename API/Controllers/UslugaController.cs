﻿using Application.DTO.Requests;
using Application.Uslugi.Commands;
using Application.Uslugi.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class UslugaController : ApiControllerBase
    {
        public UslugaController()
        {

        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetUslugaList()
        {
            return Ok(await Mediator.Send(new UslugaListQuery
            {

            }));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{ID_usluga}")]
        public async Task<IActionResult> GetUslugaDetails(string ID_usluga)
        {
            try
            {
                return Ok(await Mediator.Send(new UslugaDetailsQuery
                {
                    ID_usluga = ID_usluga
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddUsluga(UslugaRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateUslugaCommand
                {
                    request = request
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_usluga}")]
        public async Task<IActionResult> UpdateUsluga(string ID_usluga, UslugaRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateUslugaCommand
                {
                    ID_usluga = ID_usluga,
                    request = request
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}