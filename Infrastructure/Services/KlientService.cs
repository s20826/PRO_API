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
    public class KlientService : IKlientRepository
    {
        private readonly KlinikaContext context;
        private readonly IConfiguration configuration;
        public KlientService(KlinikaContext klinikaContext, IConfiguration config)
        {
            context = klinikaContext;
            configuration = config;
        }

        public async Task<List<GetKlientListResponse>> GetKlientList()
        {
            var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                select new GetKlientListResponse()
                {
                    IdOsoba = x.IdOsoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZalozeniaKonta = p.DataZalozeniaKonta
                };

            return results.ToList();
        }

        public async Task<GetKlientResponse> GetKlientById(int ID_osoba)
        {
            var results =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == ID_osoba
                select new GetKlientResponse()
                {
                    IdOsoba = x.IdOsoba,
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZalozeniaKonta = p.DataZalozeniaKonta
                };

            return results.FirstOrDefault();
        }

        public async Task<int> AddKlient(KlientCreateRequest request)
        {
            if(context.Osobas.Where(x=> x.NazwaUzytkownika.Equals(request.NazwaUzytkownika)).Any())
            {
                throw new Exception("Ta nazwa użytkownika jest już zajęta");
            }

            byte[] salt = PasswordHelper.GenerateSalt();
            string hashed = PasswordHelper.HashPassword(salt, request.Haslo, int.Parse(configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajKlienta @imie, @nazwisko, @numerTel, @email, @login, @haslo, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", request.Imie);
            command.Parameters.AddWithValue("@nazwisko", request.Nazwisko);
            command.Parameters.AddWithValue("@numerTel", request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@login", request.NazwaUzytkownika);
            command.Parameters.AddWithValue("@haslo", hashed);
            command.Parameters.AddWithValue("@salt", saltBase64);


            if (command.ExecuteNonQuery() == 2)
            {
                trans.Commit();
                await connection.CloseAsync();
                return 0;
            }
            else
            {
                trans.Rollback();
                throw new Exception("Błąd, nie udało się dodać klienta ");
            }
        }

        public async Task<int> DeleteKlient(int ID_osoba)
        {
            var user = context.Osobas.Where(x => x.IdOsoba == ID_osoba).FirstOrDefault();
            if(user != null)
            {
                throw new NotFoundException();
            }

            user.Haslo = "";
            user.Salt = "";
            user.RefreshToken = "";
            user.Email = "";
            user.NumerTelefonu = "";

            return await context.SaveChangesAsync();
        }
    }
}
