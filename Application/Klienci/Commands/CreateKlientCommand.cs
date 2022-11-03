﻿using Application.DTO;
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

namespace Application.Klienci.Commands
{
    public class CreateKlientCommand : IRequest<int>
    {
        public KlientCreateRequest request { get; set; }
    }

    public class CreateKlientCommandHandler : IRequestHandler<CreateKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        public CreateKlientCommandHandler(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IEmailSender sender)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            emailSender = sender;
        }

        public async Task<int> Handle(CreateKlientCommand req, CancellationToken cancellationToken)
        {
            if (!req.request.Haslo.Equals(req.request.Haslo2))
            {
                throw new Exception("Incorrect password");
            }
            if (context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).Any())
            {
                throw new Exception("Not unique");
            }

            byte[] salt = passwordRepository.GenerateSalt();
            string hashedPassword = passwordRepository.HashPassword(salt, req.request.Haslo, int.Parse(configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();
            SqlTransaction trans = connection.BeginTransaction();

            var query = "exec DodajKlienta @imie, @nazwisko, @numerTel, @email, @login, @haslo, @salt";
            SqlCommand command = new SqlCommand(query, connection, trans);
            command.Parameters.AddWithValue("@imie", req.request.Imie);
            command.Parameters.AddWithValue("@nazwisko", req.request.Nazwisko);
            command.Parameters.AddWithValue("@numerTel", req.request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", req.request.Email);
            command.Parameters.AddWithValue("@login", req.request.NazwaUzytkownika);
            command.Parameters.AddWithValue("@haslo", hashedPassword);
            command.Parameters.AddWithValue("@salt", saltBase64);


            if (command.ExecuteNonQuery() == 2)
            {
                trans.Commit();
                await connection.CloseAsync();
                await emailSender.SendCreateAccountEmail(req.request.Email);

                return 0;
            }
            else
            {
                trans.Rollback();
                throw new Exception("Error");
            }
        }
    }
}