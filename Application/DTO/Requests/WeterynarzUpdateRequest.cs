using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class WeterynarzUpdateRequest
    {
        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public DateTime DataUrodzenia { get; set; }

        public decimal Pensja { get; set; }

        public DateTime DataZatrudnienia { get; set; }
    }
}
