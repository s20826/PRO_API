using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetPacjentDetailsResponse
    {
        public int IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
        public string Plec { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public decimal Waga { get; set; }
        public bool Agresywne { get; set; }
        public string Wlasciciel { get; set; }
        public PacjentWizytaResponse[] Wizyty { get; set; }
    }
}
