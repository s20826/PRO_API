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

        public Account(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Not valid password");
            }

            NazwaUzytkownika = name;
        }
    }
}
