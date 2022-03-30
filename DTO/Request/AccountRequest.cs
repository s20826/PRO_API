using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.DTO.Request
{
    public class AccountRequest
    {
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public string currentHaslo { get; set; }
        public string newHaslo { get; set; }
    }
}
