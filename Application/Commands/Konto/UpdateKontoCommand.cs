using Application.DTO.Request;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Konto
{
    public class UpdateKontoCommand : IRequest<int>
    {
        public int ID_osoba { get; set; }
        public KontoUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateKontoCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        private readonly IPasswordRepository passwordRepository;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, ITokenRepository token, IPasswordRepository password)
        {
            context = klinikaContext;
            tokenRepository = token;
            passwordRepository = password;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            var user = context.Osobas.Where(x => x.IdOsoba == req.ID_osoba).FirstOrDefault();
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