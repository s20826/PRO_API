using Application.DTO.Requests;
using Application.Szczepionki.Commands;
using Application.Szczepionki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SzczepionkaController : ApiControllerBase
    {
        public SzczepionkaController()
        {

        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetSzczepionkaList()
        {
            return Ok(await Mediator.Send(new SzczepionkaListQuery
            {

            }));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_szczepionka}")]
        public async Task<IActionResult> GetSzczepionkaDetails(string ID_szczepionka)
        {
            return Ok(await Mediator.Send(new SzczepionkaDetailsQuery
            {
                ID_szczepionka = ID_szczepionka
            }));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddSzczepionka(SzczepionkaRequest request)
        {
            return Ok(await Mediator.Send(new CreateSzczepionkaCommand
            {
                request = request
            }));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_szczepionka}")]
        public async Task<IActionResult> UpdateSzczepionka(string ID_szczepionka, SzczepionkaRequest request)
        {
            return Ok(await Mediator.Send(new UpdateSzczepionkaCommand
            {
                ID_szczepionka = ID_szczepionka,
                request = request
            }));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_szczepionka}")]
        public async Task<IActionResult> DeleteSzczepionka(string ID_szczepionka)
        {
            return Ok(await Mediator.Send(new DeleteSzczepionkaCommand
            {
                ID_szczepionka = ID_szczepionka
            }));
        }
    }
}