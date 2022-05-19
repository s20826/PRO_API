using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class Zdjecie
    {
        public int IdZdjecie { get; set; }
        public int IdWizyta { get; set; }
        public byte[] Zdjecie1 { get; set; }

        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
