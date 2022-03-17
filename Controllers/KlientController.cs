using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO;
using PRO_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlientController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;

        public KlientController(IConfiguration config, KlinikaContext klinikaContext)
        {
            configuration = config;
            context = klinikaContext;
        }

        [HttpGet]
        public IActionResult GetKlientList()
        {
            var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                select new
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZalozeniaKonta
                };


            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetKlientById(int id)
        {
            if (context.Klients.Where(x => x.IdOsoba == id).Any() != true)
            {
                return BadRequest("Nie ma klienta o ID = " + id);
            } 
            else
            {
                var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == id
                select new
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    Numer_Telefonu = x.NumerTelefonu,
                    Email = x.Email,
                    Data_zalozenia_konta = p.DataZalozeniaKonta
                };

                return Ok(results.First());
            }
        }

        /*[HttpGet]
        public IActionResult GetKlienci2()
        {
            var query = "Select Imie, Nazwisko, Numer_telefonu, Email, Data_zalozenia_konta from Klient k, Osoba o Where k.ID_osoba = o.ID_osoba";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<KlientResponse> klientResponses = new List<KlientResponse>();
            while (reader.Read())
            {
                klientResponses.Add(new KlientResponse
                {
                    Imie = reader["Imie"].ToString(),
                    Nazwisko = reader["Nazwisko"].ToString(),
                    NumerTelefonu = reader["Numer_telefonu"].ToString(),
                    Email = reader["Email"].ToString(),
                    DataZalozeniaKonta = (DateTime)reader["Data_zalozenia_konta"]
                });
            }
            reader.Close();
            connection.Close();
        }*/
    }
}
