using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LoginService : ILoginRepository
    {
        public void CheckCredentails(Osoba user, IPasswordRepository passwordRepository, string haslo, int iterations)
        {
            if (user == null)
            {
                throw new UserNotAuthorizedException("Incorrect");
            }
            
            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = passwordRepository.HashPassword(salt, haslo, iterations);

            if (!passwordHash.Equals(currentHashedPassword))
            {
                throw new UserNotAuthorizedException("Incorrect");
            }
        }
    }
}
