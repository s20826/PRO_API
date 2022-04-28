using Application.DTO;
using Application.DTO.Responses;
using Application.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WeterynarzService : IWeterynarzRepository
    {
        private readonly KlinikaContext context;
        private readonly IConfiguration configuration;
        public WeterynarzService(KlinikaContext klinikaContext, IConfiguration config)
        {
            context = klinikaContext;
            configuration = config;
        }


        public async Task<List<GetWeterynarzListResponse>> GetWeterynarzList()
        {
            var results =
                from x in context.Osobas
                join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                select new GetWeterynarzListResponse()
                {
                    IdOsoba = x.IdOsoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZatrudnienia = p.DataZatrudnienia
                };

            return results.ToList();
        }


        public async Task<GetWeterynarzResponse> GetWeterynarzById(int ID_osoba)
        {
            var results =
                from x in context.Osobas
                join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == ID_osoba
                select new GetWeterynarzResponse()
                {
                    IdOsoba = x.IdOsoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZatrudnienia = p.DataZatrudnienia,
                    Pensja = p.Pensja
                };

            return results.FirstOrDefault();
        }


        public async Task<int> AddWeterynarz(WeterynarzCreateRequest request)
        {
            var salt = PasswordHelper.GenerateSalt();
            var password = PasswordHelper.HashPassword(salt, request.Haslo, int.Parse(configuration["PasswordIterations"]));

            string saltBase64 = Convert.ToBase64String(salt);

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", request.Imie);
            command.Parameters.AddWithValue("@nazwisko", request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", request.DataUrodzenia);
            command.Parameters.AddWithValue("@numerTel", request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@login", request.Login);
            command.Parameters.AddWithValue("@haslo", password);
            command.Parameters.AddWithValue("@pensja", request.Pensja);
            command.Parameters.AddWithValue("@dataZatrudnienia", request.DataZatrudnienia);
            command.Parameters.AddWithValue("@salt", saltBase64);


            if (command.ExecuteNonQuery() == 2)
            {
                await trans.CommitAsync();
                await connection.CloseAsync();
                return 0;
            }
            else
            {
                await trans.RollbackAsync();
                await connection.CloseAsync();
                throw new Exception("Error, nie udało się dodać weterynarza");
            }
        }

        public async Task<int> UpdateWeterynarzZatrudnienie(int ID_osoba, WeterynarzUpdateRequest request)
        {
            var konto = context.Osobas.Where(x => x.IdOsoba == ID_osoba).FirstOrDefault();
            var weterynarz = context.Weterynarzs.Where(x => x.IdOsoba == ID_osoba).FirstOrDefault();
            if(konto is null || weterynarz is null)
            {
                throw new Exception();
            }
            konto.Imie = request.Imie;
            konto.Nazwisko = request.Nazwisko;
            konto.NumerTelefonu = request.NumerTelefonu;
            konto.Email = request.Email;

            weterynarz.Pensja = request.Pensja;
            weterynarz.DataZatrudnienia = request.DataZatrudnienia;

            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteWeterynarz(int ID_osoba)
        {
            var weterynarz = context.Osobas.Where(x => x.IdOsoba == ID_osoba).FirstOrDefault();
            if (weterynarz != null)
            {
                throw new NotFoundException();
            }

            weterynarz.Haslo = "";
            weterynarz.Salt = "";
            weterynarz.RefreshToken = "";
            weterynarz.Email = "";
            weterynarz.NumerTelefonu = "";

            return await context.SaveChangesAsync();
        }
    }
}
