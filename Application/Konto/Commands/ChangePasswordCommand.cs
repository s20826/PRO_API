using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class ChangePasswordCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public KontoChangePasswordRequest request { get; set; }
    }

    public class ChangePasswordCommandHandle : IRequestHandler<ChangePasswordCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IHash hash;
        public ChangePasswordCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IHash _hash)
        {
            context = klinikaContext;
            passwordRepository = password;
            hash = _hash;
        }

        public async Task<int> Handle(ChangePasswordCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException();
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = await passwordRepository.GetHashed(salt, req.request.CurrentHaslo);

            if (passwordHash != currentHashedPassword)
            {
                throw new Exception("Niepoprawne hasło.");
            }

            string hashedPassword = await passwordRepository.GetHashed(salt, req.request.NewHaslo);
            user.Haslo = hashedPassword;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}