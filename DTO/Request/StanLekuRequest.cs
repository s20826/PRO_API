using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PRO_API.DTO.Request
{
    public partial class StanLekuRequest
    {
        [Required]
        [Range(1,9999)]
        public int Ilosc { get; set; }

        [Required]
        public DateTime DataWaznosci { get; set; }
    }
}
