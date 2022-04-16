using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRO_API.DTO.Request;
using PRO_API.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LekController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly KlinikaContext context;
        private readonly IHashids hashids;
        public LekController(IConfiguration config, KlinikaContext klinikaContext, IHashids ihashids)
        {
            configuration = config;
            context = klinikaContext;
            hashids = ihashids;
        }

        [HttpGet("hashid/{id}")]
        public async Task<IActionResult> GetLekList(int id)
        {
            return Ok(hashids.Encode(1) + "  " + hashids.Encode(2));
        }

        [HttpGet]
        public async Task<IActionResult> GetLekList()
        {
            var query = "Select l.ID_lek, l.Nazwa, SUM(ilosc) as Ilosc, l.Jednostka_Miary from Lek l, Lek_W_Magazynie m " +
                "where l.ID_lek = m.ID_lek AND Data_waznosci > GETDATE()" +
                "group by Nazwa, Jednostka_Miary, l.ID_lek";

            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            table.Load(reader);

            await reader.CloseAsync();
            await connection.CloseAsync();

            return Ok(table);
        }

        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek)
        {
            var idArray = hashids.Decode(ID_lek);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            if (context.Leks.Where(x => x.IdLek == id).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }

            var results =
            from x in context.Leks
            join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
            from p in ps
            where x.IdLek == id
            select new
            {
                IdStanLeku = p.IdStanLeku,
                Nazwa = x.Nazwa,
                Jednostka_Miary = x.JednostkaMiary,
                Ilosc = p.Ilosc,
                Data_Waznosci = p.DataWaznosci
            };

            return Ok(results);
        }

        [HttpGet("{ID_lek}/{ID_stan_leku}")]
        public IActionResult GetLekWMagazynieById(string ID_lek, string ID_stan_leku)
        {
            var idArray1 = hashids.Decode(ID_lek);
            var idArray2 = hashids.Decode(ID_stan_leku);
            if (idArray1.Length == 0 || idArray2.Length == 0)
            {
                return NotFound();
            }
            int id1 = idArray1[0];
            int id2 = idArray2[0];

            if (context.Leks.Where(x => x.IdLek == id1).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }

            if (context.LekWMagazynies.Where(x => x.IdStanLeku == id2).Any() != true)
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek + " w magazynie");
            }

            var results =
            from x in context.Leks
            join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
            from p in ps
            where x.IdLek == id1 && p.IdStanLeku == id2
            select new
            {
                IdStanLeku = p.IdStanLeku,
                Nazwa = x.Nazwa,
                Jednostka_Miary = x.JednostkaMiary,
                Ilosc = p.Ilosc,
                Data_Waznosci = p.DataWaznosci
            };

            return Ok(results);
        }

        [HttpPost("{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, StanLekuRequest request)
        {
            var idArray = hashids.Decode(ID_lek);
            if (idArray.Length == 0)
            {
                return NotFound();
            }
            int id = idArray[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            if (!context.Leks.Where(x => x.IdLek == id).Any())
            {
                return BadRequest("Nie ma leku o ID = " + ID_lek);
            }

            context.LekWMagazynies.Add(new LekWMagazynie
            {
                IdLek = id,
                DataWaznosci = request.DataWaznosci,
                Ilosc = request.Ilosc
            });

            await context.SaveChangesAsync();

            return Ok("Dodano informacje o leku.");
        }

        [HttpPut("{ID_lek}/{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_lek, string ID_stan_leku, StanLekuRequest request)
        {
            var idArray1 = hashids.Decode(ID_lek);
            var idArray2 = hashids.Decode(ID_stan_leku);
            if (idArray1.Length == 0 || idArray2.Length == 0)
            {
                return NotFound();
            }
            int id1 = idArray1[0];
            int id2 = idArray2[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            if (!context.LekWMagazynies.Where(x => x.IdStanLeku == id2 && x.IdLek == id1).Any())
            {
                return BadRequest("Nie ma informacji o takim leku w magazynie.");
            }

            var stanLeku = context.LekWMagazynies.Where(x => x.IdLek == id1 && x.IdStanLeku == id2).First();
            stanLeku.Ilosc = request.Ilosc;
            stanLeku.DataWaznosci = request.DataWaznosci;

            context.SaveChangesAsync();

            return Ok("Pomyślnie zaktuzalizowano dane.");
        }

        [HttpDelete("{ID_lek}/{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_lek, string ID_stan_leku)
        {
            var idArray1 = hashids.Decode(ID_lek);
            var idArray2 = hashids.Decode(ID_stan_leku);
            if (idArray1.Length == 0 || idArray2.Length == 0)
            {
                return NotFound();
            }
            int id1 = idArray1[0];
            int id2 = idArray2[0];

            if (!ModelState.IsValid)
            {
                return BadRequest("Niepoprawne dane");
            }

            if (!context.LekWMagazynies.Where(x => x.IdStanLeku == id2 && x.IdLek == id1).Any())
            {
                return BadRequest("Nie ma informacji o takim leku w magazynie.");
            }

            context.Remove(context.LekWMagazynies.Where(x => x.IdStanLeku == id2 && x.IdLek == id1).First());
            context.SaveChangesAsync();

            return Ok("Pomyślnie usunięto informacje o leku.");
        }
    }
}