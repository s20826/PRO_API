using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class KontoUpdateRequest
    {
        public string NumerTelefonu { get; set; }

        public string Email { get; set; }

        public DateTime DataUrodzenia { get; set; }
        
        public string currentHaslo { get; set; }

        public string newHaslo { get; set; }
    }
}
