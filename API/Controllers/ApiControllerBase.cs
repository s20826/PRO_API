using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected int GetUserId()
        {
            var rolesClaims = this.User.Claims.Where(x => x.Type == "idUser").ToArray();
            if (rolesClaims.ToList().Any())
            {
                return int.Parse(rolesClaims[0].Value);
            }
            else
            {
                return 0;
            }

        }
    }
}
