using Application.DTO.Request;
using Application.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IKontoRepository
    {
        public Task<GetKontoResponse> GetKonto(int ID_osoba);
        public Task<LoginTokens> Login(LoginRequest request);
        public Task<string> GetToken(Guid refreshToken);
        public Task<int> UpdateKontoCredentials(int ID_osoba, KontoUpdateRequest request);
    }
}
