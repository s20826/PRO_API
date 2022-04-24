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
        public Task<List<GetLekListResponse>> GetLekList();
        public Task<List<GetLekResponse>> GetLekById(int ID_lek);
        //public Task<> AddStanLeku(int ID_lek, StanLekuRequest request);
    }
}
