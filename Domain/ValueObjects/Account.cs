using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record Account
    {
        public string NazwaUzytkownika { get; }
        public string Haslo { get; }

        public Account(string name, string haslo)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Niepoprawna nazwa");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new PasswordFormatException();
            }

            NazwaUzytkownika = name;
            Haslo = haslo;
        }
    }
}
