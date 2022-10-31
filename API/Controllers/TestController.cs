using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Application.Interfaces;

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
        public async Task<IActionResult> GetHashedID(int id)
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            return Ok(context.Osobas.ToList());
        }

        [HttpPost("email/haslo")]
        public async Task<IActionResult> SendTestEmail()
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            try
            {
                await _emailSender.SendHasloEmail("michalostrowski00@gmail.com", "**password**");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/wizyta")]
        public async Task<IActionResult> SendTestEmail2()
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            try
            {
                var culture = new System.Globalization.CultureInfo("pl-PL");
                var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
                await _emailSender.SendUmowWizytaEmail("michalostrowski00@gmail.com", DateTime.Now.Date.ToString() + " (" + day + ") " + DateTime.Now.TimeOfDay);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
