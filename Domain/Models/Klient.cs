using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Klient
    {
        public Klient()
        {
            KlientZnizkas = new HashSet<KlientZnizka>();
            Pacjents = new HashSet<Pacjent>();
            Skierowanies = new HashSet<Skierowanie>();
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdOsoba { get; set; }
        public DateTime DataZalozeniaKonta { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
        public virtual ICollection<KlientZnizka> KlientZnizkas { get; set; }
        public virtual ICollection<Pacjent> Pacjents { get; set; }
        public virtual ICollection<Skierowanie> Skierowanies { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
