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
    }
}