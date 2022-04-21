using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class WeterynarzSpecjalizacja
    {
        public int IdOsoba { get; set; }
        public int IdSpecjalizacja { get; set; }
        public string Opis { get; set; }

        public virtual Weterynarz IdOsobaNavigation { get; set; }
        public virtual Specjalizacja IdSpecjalizacjaNavigation { get; set; }
    }
}
