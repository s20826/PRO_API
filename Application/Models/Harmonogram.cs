using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class Harmonogram
    {
        public Harmonogram()
        {
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdHarmonogram { get; set; }
        public int WeterynarzIdOsoba { get; set; }
        public int? KlientIdOsoba { get; set; }
        public int? IdPacjent { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public DateTime DataZakonczenia { get; set; }

        public virtual Pacjent IdPacjentNavigation { get; set; }
        public virtual Klient KlientIdOsobaNavigation { get; set; }
        public virtual Weterynarz WeterynarzIdOsobaNavigation { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
