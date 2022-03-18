using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Badanie
    {
        public int IdUsluga { get; set; }
        public string Dolegliwosc { get; set; }

        public virtual Usluga IdUslugaNavigation { get; set; }
    }
}
