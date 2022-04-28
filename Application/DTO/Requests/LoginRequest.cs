using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class LoginRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Pole wymaga od 8 do 30 znaków")]
        public string NazwaUzytkownika { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Pole wymaga od 8 do 30 znaków")]
        public string Haslo { get; set; }
    }
}
