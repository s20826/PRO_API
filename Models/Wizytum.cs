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
            WizytaUslugas = new HashSet<WizytaUsluga>();
        }

        public int IdWizyta { get; set; }
        public int IdPacjent { get; set; }
        public int IdOsoba { get; set; }
        public DateTime DataGodzina { get; set; }
        public string Opis { get; set; }
        public string Status { get; set; }
        public decimal Koszt { get; set; }

        public virtual Weterynarz IdOsobaNavigation { get; set; }
        public virtual Pacjent IdPacjentNavigation { get; set; }
        public virtual Receptum Receptum { get; set; }
        public virtual ICollection<LekWizytum> LekWizyta { get; set; }
        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
