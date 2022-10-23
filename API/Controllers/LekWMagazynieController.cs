using Application.DTO.Request;
using Application.LekiWMagazynie.Commands;
using Application.LekiWMagazynie.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class LekWMagazynieController : ApiControllerBase
    {
        public LekWMagazynieController()
        {
            
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieById(string ID_stan_leku)
        {
            try
            {
                return Ok(await Mediator.Send(new StanLekuQuery
                {
                    ID_stan_leku = ID_stan_leku
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost("{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, StanLekuRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateStanLekuCommand
                {
                    ID_lek = ID_lek,
                    request = request
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_stan_leku, StanLekuRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku,
                    request = request
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_stan_leku)
        {
            try
            {
                await Mediator.Send(new DeleteStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku
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