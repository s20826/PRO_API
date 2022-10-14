using Application.WeterynarzSpecjalizacje.Commands;
using Application.WeterynarzSpecjalizacje.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeterynarzSpecjalizacjaController : ApiControllerBase
    {
        public WeterynarzSpecjalizacjaController()
        {

        }

        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzSpecjalizacja(string ID_weterynarz)
        {
            try
            {
                return Ok(await Mediator.Send(new WeterynarzSpecjalizacjaListQuery
                {
                    ID_weterynarz = ID_weterynarz
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> AddSpecjalizacjaToWeterynarz(string ID_specjalizacja, string ID_weterynarz)
        {
            try
            {
                return Ok(await Mediator.Send(new AddSpecjalizacjaWeterynarzCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    ID_weterynarz = ID_weterynarz
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> RemoveSpecjalizacjaFromWeterynarz(string ID_specjalizacja, string ID_weterynarz)
        {
            try
            {
                return Ok(await Mediator.Send(new RemoveSpecjalizacjaWeterynarzCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    ID_weterynarz = ID_weterynarz
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}