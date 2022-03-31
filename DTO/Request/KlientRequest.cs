using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.DTO
{
    public class KlientRequest
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Imie { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Nazwisko { get; set; }

        [Required]
        [Display(Name = "Data Urodzenia"), DataType(DataType.Date)]
        public DateTime DataUrodzenia { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("\d{9,10}")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Pole wymaga od 9 do 10 znaków")]
        public string NumerTelefonu { get; set; }

        [EmailAddress(ErrorMessage = "Niepoprawny format e-mail")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Login { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Pole wymaga od 8 do 30 znaków")]
        public string Haslo { get; set; }
    }
}
