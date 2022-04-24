using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Szczepienie
    {
        public int IdSzczepionka { get; set; }
        public int IdWizyta { get; set; }
        public DateTime DataWaznosci { get; set; }

        public virtual Szczepionka IdSzczepionkaNavigation { get; set; }
        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
