using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Specjalizacja
    {
        public Specjalizacja()
        {
            WeterynarzSpecjalizacjas = new HashSet<WeterynarzSpecjalizacja>();
        }

        public int IdSpecjalizacja { get; set; }
        public string NazwaSpecjalizacji { get; set; }

        public virtual ICollection<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
    }
}
