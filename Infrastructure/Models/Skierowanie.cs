using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Skierowanie
    {
        public int IdOsoba { get; set; }
        public int IdUsluga { get; set; }
        public DateTime DataWystawienia { get; set; }

        public virtual Klient IdOsobaNavigation { get; set; }
        public virtual Usluga IdUslugaNavigation { get; set; }
    }
}
