using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            Skierowanies = new HashSet<Skierowanie>();
            WizytaUslugas = new HashSet<WizytaUsluga>();
        }

        public int IdUsluga { get; set; }
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public bool Narkoza { get; set; }
        public string Dolegliwosc { get; set; }

        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
