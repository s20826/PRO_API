using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public WizytaController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]   //admin
        public IActionResult GetWizytaList()
        {
            var results = context.Wizyta;
            return Ok(results);
        }
    }
}
