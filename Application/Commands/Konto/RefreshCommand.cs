using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Konto
{
    public class RefreshCommand : IRequest<string>
    {
        public Guid RefreshToken { get; set; }
    }

    public class RefreshCommandHandle : IRequestHandler<RefreshCommand, string>
    {
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        public RefreshCommandHandle(IKlinikaContext klinikaContext, ITokenRepository repository)
        {
            context = klinikaContext;
            tokenRepository = repository;
        }

        public async Task<string> Handle(RefreshCommand req, CancellationToken cancellationToken)
        {
            if (req.RefreshToken.ToString().Length == 0)
            {
                throw new NotFoundException("Nie znaleziono Refresh Token");
            }
            var user = context.Osobas.SingleOrDefault(x => x.RefreshToken == req.RefreshToken.ToString());
            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono Refresh Token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {
                throw new UserNotAuthorizedException("Refresh Token wygasł");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", user.IdOsoba.ToString()),
                new Claim("login", user.NazwaUzytkownika)
            };

            if (user.Rola != null)
            {
                if (user.Rola.Equals("A"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                if (user.Rola.Equals("W"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "weterynarz"));
                }
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "klient"));
            }

            var token = tokenRepository.GetJWT(userclaim);

            return token;
        }
    }
}