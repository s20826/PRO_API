using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record LekNazwa
    {
        public string Nazwa { get; set; }

        public LekNazwa(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa))
            {
                throw new Exception("Niepoprawna nazwa");
            }

            Nazwa = nazwa;
        }
    }
}
