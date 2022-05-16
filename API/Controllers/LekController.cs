using Application.Commands.Lek;
using Application.DTO.Request;
using Application.Queries.Lek;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class LekController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        public LekController(IConfiguration config, IHashids ihashids)
        {
            configuration = config;
            hashids = ihashids;
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetLekList()
        {
            return Ok(await Mediator.Send(new GetLekListQuery
            {

            }));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek)
        {
            var idArray = hashids.Decode(ID_lek);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            return Ok(await Mediator.Send(new GetLekQuery
            {
                ID_lek = id
            }));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieByIdAsync(string ID_stan_leku)
        {
            var idArray = hashids.Decode(ID_stan_leku);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            return Ok(await Mediator.Send(new GetStanLekuQuery
            {
                ID_stan_leku = id
            }));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("magazyn/{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, StanLekuRequest request)
        {
            var idArray = hashids.Decode(ID_lek);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            return Ok(await Mediator.Send(new CreateStanLekuCommand
            {
                ID_lek = id,
                request = request
            }));
        }

        [Authorize(Roles = "admin")]
        [HttpPut("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_stan_leku, StanLekuRequest request)
        {
            var idArray = hashids.Decode(ID_stan_leku);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            await Mediator.Send(new UpdateStanLekuCommand
            {
                ID_stan_leku = id,
                request = request
            });

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_stan_leku)
        {
            var idArray = hashids.Decode(ID_stan_leku);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            await Mediator.Send(new DeleteStanLekuCommand
            {
                ID_stan_leku = id
            });

            return NoContent();
        }
    }
}