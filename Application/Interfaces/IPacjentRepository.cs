using Application.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPacjentRepository
    {
        public Task<List<GetPacjentListResponse>> GetPacjentList();
        public Task<List<GetPacjentKlientListResponse>> GetPacjentList(int ID_osoba);
        public Task<GetPacjentDetails> GetPacjentById(int ID_pacjent);
    }
}
