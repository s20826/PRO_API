using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Wizytum
    {
        public Wizytum()
        {
            LekWizyta = new HashSet<LekWizytum>();
            Skierowanies = new HashSet<Skierowanie>();
            Szczepienies = new HashSet<Szczepienie>();
            WizytaUslugas = new HashSet<WizytaUsluga>();
        }

        public int IdWizyta { get; set; }
        public int IdHarmonogram { get; set; }
        public int? IdZnizka { get; set; }
        public string Opis { get; set; }
        public string NotatkaKlient { get; set; }
        public string Status { get; set; }
        public decimal Cena { get; set; }
        public decimal? CenaZnizka { get; set; }
        public bool CzyOplacona { get; set; }

        public virtual Harmonogram IdHarmonogramNavigation { get; set; }
        public virtual Znizka IdZnizkaNavigation { get; set; }
        public virtual Receptum Receptum { get; set; }
        public virtual ICollection<LekWizytum> LekWizyta { get; set; }
        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<Szczepienie> Szczepienies { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
