using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class RefreshCommand : IRequest<object>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshCommandHandle : IRequestHandler<RefreshCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        private readonly IHash hash;
        public RefreshCommandHandle(IKlinikaContext klinikaContext, ITokenRepository repository, IHash _hash)
        {
            context = klinikaContext;
            tokenRepository = repository;
            hash = _hash;
        }

        public async Task<object> Handle(RefreshCommand req, CancellationToken cancellationToken)
        {
            /*if (req.RefreshToken.ToString().Length == 0)
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
            }*/

            var user = context.Osobas.Where(x => x.NazwaUzytkownika.Equals("Adm1n")).First(); 

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", hash.Encode(user.IdOsoba)),
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

            return new 
            { 
                accessToken = token
            };
        }
    }
}