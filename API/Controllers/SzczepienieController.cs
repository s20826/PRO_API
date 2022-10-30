using Application.Szczepienia.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SzczepienieController : ApiControllerBase
    {
        [Authorize(Roles = "weterynarz,admin")]
        [HttpGet("{ID_pacjent}")]
        public async Task<IActionResult> GetSzczepienie(string ID_pacjent)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepieniePacjentQuery
                {
                    ID_pacjent = ID_pacjent
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpGet("{ID_szczepienie}")]
        public async Task<IActionResult> GetSzczepienieDetails(string ID_szczepienie)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepienieDetailsQuery
                {
                    ID_szczepienie = ID_szczepienie
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}