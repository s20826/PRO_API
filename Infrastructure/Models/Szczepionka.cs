using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Szczepionka
    {
        public Szczepionka()
        {
            Szczepienies = new HashSet<Szczepienie>();
        }

        public int IdSzczepionka { get; set; }
        public string Nazwa { get; set; }
        public string Zastosowanie { get; set; }

        public virtual ICollection<Szczepienie> Szczepienies { get; set; }
    }
}
