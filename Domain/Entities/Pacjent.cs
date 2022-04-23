using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pacjent : AuditableEntity, IHasDomainEvent
    {
        public uint IdPacjent { get; set; }
        public uint IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
        public Plec Plec { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public float Waga { get; set; }
        public bool Agresywne { get; set; }

        public Pacjent(uint idPacjent, uint idOsoba, string nazwa, string gatunek, string rasa, string masc, Plec plec, DateTime dataUrodzenia, float waga, bool agresywne)
        {
            IdPacjent = idPacjent;
            IdOsoba = idOsoba;
            Nazwa = nazwa;
            Gatunek = gatunek;
            Rasa = rasa;
            Masc = masc;
            Plec = plec;
            DataUrodzenia = dataUrodzenia;
            Waga = waga;
            Agresywne = agresywne;
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
