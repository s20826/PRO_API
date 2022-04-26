using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class LoginTokens
    {
        public JwtSecurityToken Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
