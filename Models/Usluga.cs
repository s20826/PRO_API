using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
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

        public virtual Badanie Badanie { get; set; }
        public virtual Zabieg Zabieg { get; set; }
        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
