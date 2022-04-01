using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PRO_API.DTO.Request
{
    public partial class WeterynarzSpecjalizacjaRequest
    {        
        [Required]
        public string Opis { get; set; }
    }
}
