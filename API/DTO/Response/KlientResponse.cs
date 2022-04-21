using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.DTO
{
    public class KlientResponse
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public DateTime DataZalozeniaKonta { get; set; }
    }
}
