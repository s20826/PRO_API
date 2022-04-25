using Application.Commands.Lek;
using Application.DTO.Request;
using Application.Queries.Lek;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PRO_API.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetLekList()
        {
            return Ok(await Mediator.Send(new GetLekListQuery
            {

            }));
        }

        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek)
        {
            var idArray = hashids.Decode(ID_lek);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            /*if (context.Leks.Where(x => x.IdLek == id).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }*/

            return Ok(await Mediator.Send(new GetLekQuery
            {
                ID_lek = id
            }));
        }

        [HttpGet("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieByIdAsync(string ID_stan_leku)
        {
            var idArray = hashids.Decode(ID_stan_leku);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            /*if (context.Leks.Where(x => x.IdLek == id1).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }

            if (context.LekWMagazynies.Where(x => x.IdStanLeku == id2).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek + " w magazynie");
            }*/

            return Ok(await Mediator.Send(new GetStanLekuQuery
            {
                ID_stan_leku = id
            }));
        }

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

            await Mediator.Send(new CreateStanLekuCommand
            {
                ID_lek = id,
                request = request
            });

            return NoContent();
        }

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

            /*if (!context.LekWMagazynies.Where(x => x.IdStanLeku == id2 && x.IdLek == id1).Any())
            {
                return BadRequest("Nie ma informacji o takim leku w magazynie.");
            }*/

            await Mediator.Send(new UpdateStanLekuCommand
            {
                ID_stan_leku = id,
                request = request
            });

            return NoContent();
        }

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

            /*if (!context.LekWMagazynies.Where(x => x.IdStanLeku == id).Any())
            {
                return BadRequest("Nie ma informacji o takim leku w magazynie.");
            }*/

            await Mediator.Send(new DeleteStanLekuCommand
            {
                ID_stan_leku = id
            });

            return NoContent();
        }
    }
}