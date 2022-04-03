using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PRO_API.Models
{
    public partial class PacjentRequest
    {
        [Required]
        public int IdOsoba { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Gatunek { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Rasa { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Masc { get; set; }

        [Required]
        [RegularExpression(@"M|F")]
        public string Plec { get; set; }

        [Required]
        public DateTime DataUrodzenia { get; set; }

        [Required]
        [Range(0,999)]
        public int Waga { get; set; }

        [Required]
        public bool Agresywne { get; set; }
    }
}
