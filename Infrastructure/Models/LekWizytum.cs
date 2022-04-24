using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class LekWizytum
    {
        public int IdWizyta { get; set; }
        public int IdLek { get; set; }
        public int Ilosc { get; set; }

        public virtual Lek IdLekNavigation { get; set; }
        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
