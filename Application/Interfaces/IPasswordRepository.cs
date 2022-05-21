using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPasswordRepository
    {
        public Task<string> GetHashed(byte[] salt, string plainPassword);
        public Task<(byte[], string)> GetHashed(string plainPassword);
    }
}
