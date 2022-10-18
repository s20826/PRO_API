using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.DTO.Requests;
using Application.Konto.Commands;
using Application.Konto.Queries;
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
        [HttpPut]
        public async Task<IActionResult> UpdateKontoCredentials(KontoUpdateRequest request)
        {
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


        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> ChangeKontoPassword(KontoChangePasswordRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new ChangePasswordCommand
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