using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class WizytaChoroba
    {
        public int IdWizyta { get; set; }
        public int IdChoroba { get; set; }

        public virtual Choroba IdChorobaNavigation { get; set; }
        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
