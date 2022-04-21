using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Weterynarz
    {
        public Weterynarz()
        {
            WeterynarzSpecjalizacjas = new HashSet<WeterynarzSpecjalizacja>();
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdOsoba { get; set; }
        public decimal Pensja { get; set; }
        public DateTime DataZatrudnienia { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
        public virtual ICollection<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
