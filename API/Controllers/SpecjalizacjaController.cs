using Application.DTO.Request;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SpecjalizacjaController : ApiControllerBase
    {

        public SpecjalizacjaController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetSpecjalizacjaList()
        {
            return Ok(await Mediator.Send(new SpecjalizacjaListQuery
            {

            }));
        }

        [HttpGet("details/{ID_specjalizacja}")]
        public async Task<IActionResult> GetSpecjalizacjaById(string ID_specjalizacja)
        {
            try 
            {
                return Ok(await Mediator.Send(new SpecjalizacjaDetailsQuery
                {
                    ID_specjalizacja = ID_specjalizacja
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSpecjalizacja(SpecjalizacjaRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateSpecjalizacjaCommand
                {
                    request = request
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut("{ID_specjalizacja}")]
        public async Task<IActionResult> UpdateSpecjalizacja(string ID_specjalizacja, SpecjalizacjaRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateSpecjalizacjaCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    request = request
                }));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete("{ID_specjalizacja}")]
        public async Task<IActionResult> DeleteSpecjalizacja(string ID_specjalizacja)
        {
            try
            {
                await Mediator.Send(new DeleteSpecjalizacjaCommand
                {
                    ID_specjalizacja = ID_specjalizacja
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
