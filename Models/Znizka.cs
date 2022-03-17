using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Znizka
    {
        public Znizka()
        {
            KlientZnizkas = new HashSet<KlientZnizka>();
        }

        public int IdZnizka { get; set; }
        public string NazwaZnizki { get; set; }
        public float ProcentZnizki { get; set; }

        public virtual ICollection<KlientZnizka> KlientZnizkas { get; set; }
    }
}
