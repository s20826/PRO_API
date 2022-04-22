using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILekRepository
    { 
        public Task GetLekList();
        public Task GetLekById(string ID_lek);
    }
}
