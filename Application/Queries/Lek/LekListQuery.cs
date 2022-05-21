using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Lek
{
    public class LekListQuery : IRequest<List<GetLekListResponse>>
    {
       
    }

    public class GetLekListQueryHandle : IRequestHandler<LekListQuery, List<GetLekListResponse>>
    {
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        public GetLekListQueryHandle(IConfiguration config, IHash _hash)
        {
            configuration = config;
            hash = _hash;
        }

        public async Task<List<GetLekListResponse>> Handle(LekListQuery req, CancellationToken cancellationToken)
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
                    IdLek = hash.Encode(int.Parse(reader["ID_lek"].ToString())),
                    Nazwa = reader["Nazwa"].ToString(),
                    Ilosc = (uint)int.Parse(reader["Ilosc"].ToString()),
                    JednostkaMiary = reader["Jednostka_Miary"].ToString()
                });
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return list;
        }
    }
}
