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
using System.Threading;
using Microsoft.Extensions.Logging;
using Application.DTO.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using Microsoft.IdentityModel.Tokens;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        private readonly KlinikaContext context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TestController> logger;
        public TestController(IEmailSender emailSender, IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext, ILogger<TestController> _logger)
        {
            _emailSender = emailSender;
            configuration = config;
            hashids = ihashids;
            context = klinikaContext;
            logger = _logger;
        }
        

        [HttpGet("measureInclude")]
        public async Task<IActionResult> Test1(CancellationToken token)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                
                for(int i=0; i <20; i++)
                {
                context.Wizyta
                .Include(x => x.Harmonograms).ThenInclude(y => y.WeterynarzIdOsobaNavigation)
                .Include(x => x.IdPacjentNavigation!)
                .Include(x => x.IdOsobaNavigation)
                .Select(g => new GetWizytaListResponse()
                {
                    IdWizyta = hashids.Encode(g.IdWizyta),
                    IdPacjent = g.IdPacjent != null ? hashids.Encode((int)g.IdPacjent) : null,
                    Pacjent = g.IdPacjent != null ? g.IdPacjentNavigation.Nazwa : null,
                    IdKlient = g.IdOsoba != null ? hashids.Encode(g.IdOsoba) : "",
                    Klient = g.IdOsobaNavigation.IdOsobaNavigation.Imie + " " + g.IdOsobaNavigation.IdOsobaNavigation.Nazwisko,
                    IdWeterynarz = !g.Harmonograms.IsNullOrEmpty() ? hashids.Encode(g.Harmonograms.First().WeterynarzIdOsoba) : null,
                    Weterynarz = !g.Harmonograms.IsNullOrEmpty() ? 
                        g.Harmonograms.First().WeterynarzIdOsobaNavigation.IdOsobaNavigation.Imie + " " + g.Harmonograms.First().WeterynarzIdOsobaNavigation.IdOsobaNavigation.Nazwisko : "",
                    Status = g.Status,
                    CzyOplacona = g.CzyOplacona,
                    CzyZaakceptowanaCena = g.CzyZaakceptowanaCena,
                    Data = g.Harmonograms.IsNullOrEmpty() ? g.Harmonograms
                        .OrderBy(x => x.DataRozpoczecia)
                        .Select(x => x.DataRozpoczecia).First() : null
                })
                .ToList()
                .OrderByDescending(x => x.Data)
                .ToList();
                }

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                return Ok(new
                {
                    time = elapsedMs
                });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("measureQuery")]
        public async Task<IActionResult> Test2(CancellationToken token)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 0; i < 20; i++)
                {
                    (from x in context.Wizyta
                     join d in context.Harmonograms on x.IdWizyta equals d.IdWizyta into harmonogram
                     from y in harmonogram.DefaultIfEmpty()
                     join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                     join d in context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                     from p in pacjent.DefaultIfEmpty()
                     group x by new { x.IdWizyta, x.IdOsoba, x.IdPacjent, x.Status, x.CzyOplacona, x.CzyZaakceptowanaCena, p.Nazwa, y.WeterynarzIdOsoba, k.Imie, k.Nazwisko }
                    into g
                     select new GetWizytaListResponse()
                     {
                         IdWizyta = hashids.Encode(g.Key.IdWizyta),
                         IdPacjent = g.Key.IdPacjent != null ? hashids.Encode((int)g.Key.IdPacjent) : null,
                         Pacjent = g.Key.IdPacjent != null ? g.Key.Nazwa : null,
                         IdKlient = g.Key.IdOsoba != null ? hashids.Encode(g.Key.IdOsoba) : "",
                         Klient = g.Key.Imie + " " + g.Key.Nazwisko,
                         IdWeterynarz = g.Key.WeterynarzIdOsoba != null ? hashids.Encode(g.Key.WeterynarzIdOsoba) : null,
                         Weterynarz = g.Key.WeterynarzIdOsoba != null ? context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                         Status = g.Key.Status,
                         CzyOplacona = g.Key.CzyOplacona,
                         CzyZaakceptowanaCena = g.Key.CzyZaakceptowanaCena,
                         Data = g.Key.WeterynarzIdOsoba != null ? context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).OrderBy(x => x.DataRozpoczecia).Select(x => x.DataRozpoczecia).First() : null
                     })
                     .AsParallel()
                     .WithCancellation(token)
                     .ToList()
                     .OrderByDescending(x => x.Data)
                     .ToList();

                }

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                return Ok(new
                {
                    time = elapsedMs
                });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("cancellation")]
        public async Task<IActionResult> CancellationTest(CancellationToken token)
        {
            try
            {
                await Task.Delay(3000, token);
                var result = context.Osobas.AsParallel().WithCancellation(token).ToList();
                logger.LogWarning(result.Count().ToString());
                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                logger.LogWarning("success, task was cancelled");
                return Ok();
            }

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