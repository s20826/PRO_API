using Application.Commands.Konto;
using Application.DTO.Request;
using Application.Queries.Konto;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KontoController : ApiControllerBase
    {
        public KontoController()
        {

        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetKonto()
        {
            return Ok(await Mediator.Send(new KontoQuery
            {
                ID_osoba = GetUserId()
            }));
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Mediator.Send(new LoginCommand
                {
                    request = request
                }));
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case UserNotAuthorizedException:
                        return Unauthorized(new
                        {
                            message = e.Message
                        });
                    default:
                        return NotFound(new
                        {
                            message = e.Message
                        });
                }
            }
        }


        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> GetToken(Guid refreshToken)
        {
            return Ok(await Mediator.Send(new RefreshCommand
            {
                RefreshToken = refreshToken
            }));
        }


        [Authorize]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateKontoCredentials(KontoUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Mediator.Send(new UpdateKontoCommand
                {
                    ID_osoba = GetUserId(),
                    request = request
                }));
            }
            catch (Exception e)
            {
                return NotFound(new
                {
                    message = e.Message
                });
            }
        }
    }
}
