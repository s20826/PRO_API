using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Osoba
    {
        public int IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Haslo { get; set; }
        public string Salt { get; set; }

        public virtual Klient Klient { get; set; }
        public virtual Weterynarz Weterynarz { get; set; }
    }
}
