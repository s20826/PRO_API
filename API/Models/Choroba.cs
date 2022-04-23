using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Choroba
    {
        public Choroba()
        {
            ChorobaLeks = new HashSet<ChorobaLek>();
        }

        public int IdChoroba { get; set; }
        public string Nazwa { get; set; }

        public virtual ICollection<ChorobaLek> ChorobaLeks { get; set; }
    }
}
