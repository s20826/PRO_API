using Application.Commands.Weterynarz;
using Application.DTO;
using Application.Queries.Weterynarz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeterynarzController : ApiControllerBase
    {
        private readonly IConfiguration configuration;

        public WeterynarzController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeterynarzList()
        {
            return Ok(await Mediator.Send(new GetWeterynarzListQuery
            {
                
            }));
        }

        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWeterynarzById(int ID_osoba)
        {
            return Ok(await Mediator.Send(new GetWeterynarzQuery
            {
                ID_osoba = ID_osoba
            }));
        }

        [HttpPost]
        public async Task<IActionResult> AddWeterynarz(WeterynarzCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            return Ok(await Mediator.Send(new CreateWeterynarzCommand
            {
                request = request
            }));
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateWeterynarzZatrudnienie(int ID_osoba, WeterynarzUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await Mediator.Send(new UpdateWeterynarzCommand
                {
                    ID_osoba = ID_osoba,
                    request = request
                }));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteWeterynarz(int ID_osoba)
        {
            await Mediator.Send(new DeleteWeterynarzCommand
            {
                ID_osoba = ID_osoba
            });

            return NoContent();
        }

        /*[Authorize(Roles = "admin")]
        [HttpPut("{ID_osoba}")]
        public IActionResult UpdateWeterynarz(int ID_osoba, KlientPutRequest request)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any())
            {
                return BadRequest("Nie ma konta o ID = " + ID_osoba);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).First();
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;

            context.SaveChanges();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }*/
    }
}
