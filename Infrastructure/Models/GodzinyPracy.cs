using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class GodzinyPracy
    {
        public int IdGodzinyPracy { get; set; }
        public int IdOsoba { get; set; }
        public string DzienTygodnia { get; set; }
        public TimeSpan GodzinaRozpoczecia { get; set; }
        public TimeSpan GodzinaZakonczenia { get; set; }

        public virtual 
            Weterynarz IdOsobaNavigation { get; set; }
    }
}
