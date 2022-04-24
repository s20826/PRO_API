using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Zabieg
    {
        public int IdUsluga { get; set; }
        public bool Narkoza { get; set; }

        public virtual Usluga IdUslugaNavigation { get; set; }
    }
}
