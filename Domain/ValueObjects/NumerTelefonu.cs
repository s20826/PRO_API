using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record NumerTelefonu
    {
        public string Numer { get; set; }

        public NumerTelefonu(string numer)
        {
            if (string.IsNullOrWhiteSpace(numer))
            {
                throw new Exception("Niepoprawna nazwa");
            }
            if(numer.Length >= 9 && numer.Length <= 10)
            {
                throw new Exception("Niepoprawny format numeru telefonu");
            }

            Numer = numer;
        }
    }
}
