using Application.DTO;
using Application.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IKlientRepository
    {
        public Task<List<GetKlientListResponse>> GetKlientList();
        public Task<GetKlientResponse> GetKlientById(int ID_osoba);
        public Task<int> AddKlient(KlientCreateRequest request);
    }
}
