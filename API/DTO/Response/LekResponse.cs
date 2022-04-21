using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class LekResponse
    {
        public int IdLek { get; set; }
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
        public string JednostkaMiary { get; set; }
    }
}
