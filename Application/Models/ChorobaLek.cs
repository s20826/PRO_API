using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class ChorobaLek
    {
        public int IdChoroba { get; set; }
        public int IdLek { get; set; }

        public virtual Choroba IdChorobaNavigation { get; set; }
        public virtual Lek IdLekNavigation { get; set; }
    }
}
