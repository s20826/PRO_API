using Application.Commands.Konto;
using Application.DTO.Request;
using Application.Queries.Konto;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KontoController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        public KontoController(IConfiguration config)
        {
            configuration = config;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetKonto()
        {
            return Ok(await Mediator.Send(new GetKontoQuery
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
        public async Task<IActionResult> UpdateKontoCredentials(int ID_osoba, KontoUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await Mediator.Send(new UpdateKontoCommand
            {
                ID_osoba = ID_osoba,
                request = request
            }));
        }
    }
}
