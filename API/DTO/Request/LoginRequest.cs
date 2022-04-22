﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.DTO.Request
{
    public class LoginRequest
    {
        [Required]
        public string NazwaUzytkownika { get; set; }

        [Required]
        public string Haslo { get; set; }
    }
}