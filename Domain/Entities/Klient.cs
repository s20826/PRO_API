using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    class Klient
    {
        public int IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        
        public string Haslo { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
        public DateTime DataZalozeniaKonta { get; set; }

        public Account account { get; }
    }
}
