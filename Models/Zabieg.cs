using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Zabieg
    {
        public int IdUsluga { get; set; }
        public bool Narkoza { get; set; }

        public virtual Usluga IdUslugaNavigation { get; set; }
    }
}
