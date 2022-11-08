﻿using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class UpdateKontoCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public KontoUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateKontoCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly ILoginRepository loginRepository;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IHash _hash, ILoginRepository login)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            loginRepository = login;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (!loginRepository.CheckCredentails(user, passwordRepository, req.request.Haslo, int.Parse(configuration["PasswordIterations"])))
            {
                await context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect");
            }

            user.NumerTelefonu = req.request.NumerTelefonu;
            user.Email = req.request.Email;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}