using Application.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILekRepository
    { 
        public Task<GetLekListResponse> GetLekList();
        public Task<GetLekResponse> GetLekById(string ID_lek);
    }
}
