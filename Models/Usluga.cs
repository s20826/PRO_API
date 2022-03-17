using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            Badanies = new HashSet<Badanie>();
            Skierowanies = new HashSet<Skierowanie>();
            WizytaUslugas = new HashSet<WizytaUsluga>();
            Zabiegs = new HashSet<Zabieg>();
        }

        public int IdUsluga { get; set; }
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }

        public virtual ICollection<Badanie> Badanies { get; set; }
        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
        public virtual ICollection<Zabieg> Zabiegs { get; set; }
    }
}
