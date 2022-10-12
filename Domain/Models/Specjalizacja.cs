using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Specjalizacja
    {
        public int IdSpecjalizacja { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
    }
}
