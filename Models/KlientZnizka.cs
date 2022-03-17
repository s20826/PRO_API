using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class KlientZnizka
    {
        public int KlientIdOsoba { get; set; }
        public int ZnizkaIdZnizka { get; set; }
        public DateTime? DataPrzyznania { get; set; }
        public bool CzyWykorzystana { get; set; }

        public virtual Klient KlientIdOsobaNavigation { get; set; }
        public virtual Znizka ZnizkaIdZnizkaNavigation { get; set; }
    }
}
