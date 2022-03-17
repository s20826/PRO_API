using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Klient
    {
        public Klient()
        {
            KlientZnizkas = new HashSet<KlientZnizka>();
            Pacjents = new HashSet<Pacjent>();
        }

        public int IdOsoba { get; set; }
        public DateTime DataZalozeniaKonta { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
        public virtual ICollection<KlientZnizka> KlientZnizkas { get; set; }
        public virtual ICollection<Pacjent> Pacjents { get; set; }
    }
}
