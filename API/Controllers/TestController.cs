using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using System.Net.Mail;
using Application.DTO.Responses;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        private readonly KlinikaContext context;
        public TestController(IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext)
        {
            configuration = config;
            hashids = ihashids;
            context = klinikaContext;
        }

        [HttpGet("hashid/{id}")]
        public async Task<IActionResult> GetHashedID(int id)        //test
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("password")]
        public async Task<IActionResult> GetHashedPassword()        //test
        {
            var salt = PasswordHelper.GenerateSalt();
            var myPassword = PasswordHelper.HashPassword(salt, "SecretPassword123", 20000);
            return Ok(new {
                PasswordHelper = myPassword,
                Length = myPassword.Length
            });
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()        //test
        {
            return Ok(context.Osobas.ToList());
        }

        [HttpGet("leki")]
        public async Task<IActionResult> GetLeki()        //test
        {
            var results = (from x in context.Leks
                        join y in context.LekWMagazynies on x.IdLek equals y.IdLek
                        select new GetLekResponse()
                        {
                            IdStanLeku = (uint)y.IdStanLeku,
                            Nazwa = x.Nazwa,
                            JednostkaMiary = x.JednostkaMiary,
                            Ilosc = (uint)y.Ilosc,
                            DataWaznosci = y.DataWaznosci,
                            Choroby = (from i in context.ChorobaLeks
                                       join j in context.Chorobas on i.IdChoroba equals j.IdChoroba into qs
                                       from j in qs.DefaultIfEmpty()
                                       where i.IdLek == x.IdLek
                                       select new
                                       {
                                           j.Nazwa
                                       }).ToArray()
                        }).ToList();

            return Ok(results);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPassword(int count)        //test
        {
            return Ok(PasswordHelper.GetRandomPassword(count));
        }
    }
}
