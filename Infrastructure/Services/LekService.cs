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
            var query = "Select l.ID_lek, l.Nazwa, SUM(ilosc) as Ilosc, l.Jednostka_Miary from Lek l, Lek_W_Magazynie m " +
                "where l.ID_lek = m.ID_lek AND Data_waznosci > GETDATE()" +
                "group by Nazwa, Jednostka_Miary, l.ID_lek";

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
            select new 
            {
                IdStanLeku = (uint)p.IdStanLeku,
                Nazwa = x.Nazwa,
                JednostkaMiary = x.JednostkaMiary,
                Ilosc = (uint)p.Ilosc,
                DataWaznosci = p.DataWaznosci
            };

            var list = new List<GetLekResponse>();
            foreach (var x in results)
            {
                list.Add(new GetLekResponse
                {
                    IdStanLeku = x.IdStanLeku,
                    Nazwa = x.Nazwa,
                    JednostkaMiary = x.JednostkaMiary,
                    Ilosc = x.Ilosc,
                    DataWaznosci = x.DataWaznosci
                });
            }

            return list;
        }
    }
}
