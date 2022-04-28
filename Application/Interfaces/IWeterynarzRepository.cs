using Application.DTO;
using Application.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWeterynarzRepository
    {
        public Task<List<GetWeterynarzListResponse>> GetWeterynarzList();
        public Task<GetWeterynarzResponse> GetWeterynarzById(int ID_osoba);
        public Task<int> AddWeterynarz(WeterynarzCreateRequest request);
        public Task<int> UpdateWeterynarzZatrudnienie(int ID_osoba, WeterynarzUpdateRequest request);
        public Task<int> DeleteWeterynarz(int ID_osoba);
    }
}
