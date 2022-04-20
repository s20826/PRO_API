﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.DTO.Request
{
    public class AccountCredentialsRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string NumerTelefonu { get; set; }

        [EmailAddress(ErrorMessage = "Niepoprawny format e-mail")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 50 znaków")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Pole wymaga od 8 do 30 znaków")]
        public string currentHaslo { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Pole wymaga od 8 do 30 znaków")]
        public string newHaslo { get; set; }
    }
}