using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Nagrodum
    {
        public int IdNagroda { get; set; }
        public int IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }

        public virtual Weterynarz IdOsobaNavigation { get; set; }
    }
}
