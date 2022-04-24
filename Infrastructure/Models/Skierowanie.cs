using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Skierowanie
    {
        public int IdUsluga { get; set; }
        public int IdWizyta { get; set; }
        public DateTime DataWystawienia { get; set; }

        public virtual Usluga IdUslugaNavigation { get; set; }
        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
