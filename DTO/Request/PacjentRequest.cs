using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class PacjentRequest
    {
        public int IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
        public string Plec { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public int Waga { get; set; }
        public bool Agresywne { get; set; }
    }
}
