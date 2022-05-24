using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Models
{
    public partial class WeterynarzSpecjalizacja
    {
        public int IdOsoba { get; set; }
        public int IdSpecjalizacja { get; set; }
        public DateTime? DataUzyskania { get; set; }
    }
}
