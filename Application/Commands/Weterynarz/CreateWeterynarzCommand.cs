using Application.DTO;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Weterynarz
{
    public class CreateWeterynarzCommand : IRequest<int>
    {
        public WeterynarzCreateRequest request { get; set; }
    }

    public class CreateWeterynarzCommandHandle : IRequestHandler<CreateWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        public CreateWeterynarzCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
        }

        public async Task<int> Handle(CreateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            if (context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.Login)).Any())
            {
                throw new Exception("Ta nazwa użytkownika jest już zajęta");
            }
            var result = await passwordRepository.GetHashed(req.request.Haslo);
            byte[] salt = result.Item1;
            string hashedPassword = result.Item2;
            string saltBase64 = Convert.ToBase64String(salt);

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", req.request.Imie);
            command.Parameters.AddWithValue("@nazwisko", req.request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", req.request.DataUrodzenia);
            command.Parameters.AddWithValue("@numerTel", req.request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", req.request.Email);
            command.Parameters.AddWithValue("@login", req.request.Login);
            command.Parameters.AddWithValue("@haslo", hashedPassword);
            command.Parameters.AddWithValue("@pensja", req.request.Pensja);
            command.Parameters.AddWithValue("@dataZatrudnienia", req.request.DataZatrudnienia);
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
    }
}