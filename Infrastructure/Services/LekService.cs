using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LekService : ILekRepository
    {
        private readonly KlinikaContext context;
        private readonly IConfiguration configuration;
        public LekService(KlinikaContext klinikaContext, IConfiguration config)
        {
            context = klinikaContext;
            configuration = config;
        }

        public async Task<List<GetLekListResponse>> GetLekList()
        {
            var query = "SELECT l.ID_lek, l.Nazwa, SUM(ilosc) AS Ilosc, l.Jednostka_Miary FROM Lek l, Lek_W_Magazynie m " +
                "WHERE l.ID_lek = m.ID_lek AND Data_waznosci > GETDATE()" +
                "GROUP BY Nazwa, Jednostka_Miary, l.ID_lek";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            var list = new List<GetLekListResponse>();

            while (reader.Read())
            {
                list.Add(new GetLekListResponse
                {
                    IdLek = (uint)int.Parse(reader["ID_lek"].ToString()),
                    Nazwa = reader["Nazwa"].ToString(),
                    Ilosc = (uint)int.Parse(reader["Ilosc"].ToString()),
                    JednostkaMiary = reader["Jednostka_Miary"].ToString()
                });
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return list;
        }

        public async Task<List<GetLekResponse>> GetLekById(int ID_lek)
        {
            var results =
            from x in context.Leks
            join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
            from p in ps
            where x.IdLek == ID_lek
            select new GetLekResponse()
            {
                IdStanLeku = (uint)p.IdStanLeku,
                Nazwa = x.Nazwa,
                JednostkaMiary = x.JednostkaMiary,
                Ilosc = (uint)p.Ilosc,
                DataWaznosci = p.DataWaznosci
            };

            return results.ToList();
        }

        public async Task<GetStanLekuResponse> GetLekWMagazynieById(int ID_stan_leku)
        {
            var result =
            from x in context.Leks
            join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
            from p in ps
            where p.IdStanLeku == ID_stan_leku
            select new GetStanLekuResponse()
            {
                IdStanLeku = (uint)p.IdStanLeku,
                Nazwa = x.Nazwa,
                JednostkaMiary = x.JednostkaMiary,
                Ilosc = (uint)p.Ilosc,
                DataWaznosci = p.DataWaznosci
            };

            return result.FirstOrDefault();
        }

        public async Task<int> AddStanLeku(int ID_lek, StanLekuRequest request)
        {
            context.LekWMagazynies.Add(new LekWMagazynie
            {
                IdLek = ID_lek,
                DataWaznosci = request.DataWaznosci,
                Ilosc = request.Ilosc
            });

            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateStanLeku(int ID_stan_leku, StanLekuRequest request)
        {
            var stanLeku = context.LekWMagazynies.Where(x => x.IdStanLeku == ID_stan_leku).FirstOrDefault();
            stanLeku.Ilosc = request.Ilosc;
            stanLeku.DataWaznosci = request.DataWaznosci;

            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteStanLeku(int ID_stan_leku)
        {
            context.Remove(context.LekWMagazynies.Where(x => x.IdStanLeku == ID_stan_leku).First());
            return await context.SaveChangesAsync();
        }
    }
}
