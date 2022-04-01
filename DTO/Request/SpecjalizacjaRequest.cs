using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PRO_API.DTO.Request
{
    public partial class SpecjalizacjaRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string NazwaSpecjalizacji { get; set; }
    }
}
