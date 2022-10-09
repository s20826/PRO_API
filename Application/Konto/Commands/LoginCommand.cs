using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class LoginCommand : IRequest<LoginTokens>
    {
        public LoginRequest request { get; set; }
    }

    public class LoginCommandHandle : IRequestHandler<LoginCommand, LoginTokens>
    {
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        private readonly IPasswordRepository passwordRepository;
        private readonly IHash hash;
        public LoginCommandHandle(IKlinikaContext klinikaContext, ITokenRepository token, IPasswordRepository password, IHash _hash)
        {
            context = klinikaContext;
            tokenRepository = token;
            passwordRepository = password;
            hash = _hash;
        }

        public async Task<LoginTokens> Handle(LoginCommand req, CancellationToken cancellationToken)
        {
            var user = context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotAuthorizedException("Niepoprawne hasło lub login.");
            }

            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = await passwordRepository.GetHashed(salt, req.request.Haslo);

            if (passwordHash != currentHashedPassword)
            {
                throw new UserNotAuthorizedException("Niepoprawne hasło lub login. ");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", hash.Encode(user.IdOsoba)),
                new Claim("login", user.NazwaUzytkownika)
            };

            string userRola = "";

            if (user.Rola != null)
            {
                if (user.Rola.Equals("A"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "admin"));
                    userRola = "admin";
                }
                if (user.Rola.Equals("W"))
                {
                    userclaim.Add(new Claim(ClaimTypes.Role, "weterynarz"));
                    userRola = "weterynarz";
                }
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "klient"));
                userRola = "user";
            }

            var token = tokenRepository.GetJWT(userclaim);

            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(3);
            await context.SaveChangesAsync(cancellationToken);

            return new LoginTokens()
            {
                Token = token,
                RefreshToken = refreshToken,
                Imie = user.Imie,
                Rola = userRola
            };
        }
    }
}
