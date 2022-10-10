﻿using Application.DTO.Request;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly ITokenRepository tokenRepository;
        private readonly IPasswordRepository passwordRepository;
        private readonly IHash hash;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, ITokenRepository token, IPasswordRepository password, IHash _hash)
        {
            context = klinikaContext;
            tokenRepository = token;
            passwordRepository = password;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException();
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = await passwordRepository.GetHashed(salt, req.request.currentHaslo);

            if (passwordHash != currentHashedPassword)
            {
                throw new Exception("Niepoprawne hasło.");
            }

            string hashedPassword = await passwordRepository.GetHashed(salt, req.request.newHaslo);

            user.NumerTelefonu = req.request.NumerTelefonu;
            user.Email = req.request.Email;
            user.Haslo = hashedPassword;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}