using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Application.Interfaces;
using System.Net.Mail;
using System.Net;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        private readonly KlinikaContext context;
        private readonly IEmailSender _emailSender;
        public TestController(IEmailSender emailSender, IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext)
        {
            _emailSender = emailSender;
            configuration = config;
            hashids = ihashids;
            context = klinikaContext;
        }

        [HttpGet("hashid/{id}")]
        public IActionResult GetHashedID(int id)
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            return Ok(context.Osobas.ToList());
        }

        [HttpPost("email/haslo")]
        public async Task<IActionResult> SendTestEmail()
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            try
            {
                await _emailSender.SendHasloEmail("to@example.com", "**password**");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/umowWizyte")]
        public async Task<IActionResult> SendTestEmail2()
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            try
            {
                await _emailSender.SendUmowWizytaEmail("to@example.com", DateTime.Now, "Zbigniew Nowak");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/anulujWizyte")]
        public async Task<IActionResult> SendTestEmail3()
        {
            try
            {
                await _emailSender.SendAnulujWizyteEmail("to@example.com", DateTime.Now);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/createAccount")]
        public async Task<IActionResult> SendTestEmail4()
        {
            try
            {
                await _emailSender.SendCreateAccountEmail("to@example.com");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}