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
            return Ok(await Mediator.Send(new LekListQuery
            {

            }));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek)
        {
            try
            {
                return Ok(await Mediator.Send(new LekQuery
                {
                    ID_lek = ID_lek
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieByIdAsync(string ID_stan_leku)
        {
            try
            {
                return Ok(await Mediator.Send(new StanLekuQuery
                {
                    ID_stan_leku = ID_stan_leku
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("magazyn/{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, StanLekuRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            try
            {
                return Ok(await Mediator.Send(new CreateStanLekuCommand
                {
                    ID_lek = ID_lek,
                    request = request
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_stan_leku, StanLekuRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            try { 
                await Mediator.Send(new UpdateStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku,
                    request = request
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("magazyn/{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_stan_leku)
        {
            try
            {
                await Mediator.Send(new DeleteStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku
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