using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Weterynarz
    {
        public Weterynarz()
        {
            Harmonograms = new HashSet<Harmonogram>();
            Nagroda = new HashSet<Nagrodum>();
            WeterynarzSpecjalizacjas = new HashSet<WeterynarzSpecjalizacja>();
        }

        public int IdOsoba { get; set; }
        public decimal Pensja { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public DateTime DataZatrudnienia { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
        public virtual ICollection<Harmonogram> Harmonograms { get; set; }
        public virtual ICollection<Nagrodum> Nagroda { get; set; }
        public virtual ICollection<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
    }
}
