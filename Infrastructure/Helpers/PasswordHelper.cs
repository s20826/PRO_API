using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public abstract class PasswordHelper
    {
        public static String HashPassword(byte[] salt, string plainPassword, int iterations)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: plainPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterations,
                numBytesRequested: 512 / 8));
            return hashedPassword;
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string GetRandomPassword(int l)
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder password = new StringBuilder();
            for(int i = 0; i < l; i++)
            {
                password.Append(valid.ElementAt(new Random().Next(0,valid.Length)));
            }
            return password.ToString();
        }
    }
}
