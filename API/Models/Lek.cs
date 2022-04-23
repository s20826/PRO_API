using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class Lek
    {
        public Lek()
        {
            ChorobaLeks = new HashSet<ChorobaLek>();
            LekWMagazynies = new HashSet<LekWMagazynie>();
            LekWizyta = new HashSet<LekWizytum>();
            ReceptaLeks = new HashSet<ReceptaLek>();
        }

        public int IdLek { get; set; }
        public string Nazwa { get; set; }
        public string JednostkaMiary { get; set; }

        public virtual ICollection<ChorobaLek> ChorobaLeks { get; set; }
        public virtual ICollection<LekWMagazynie> LekWMagazynies { get; set; }
        public virtual ICollection<LekWizytum> LekWizyta { get; set; }
        public virtual ICollection<ReceptaLek> ReceptaLeks { get; set; }
    }
}
