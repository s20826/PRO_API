using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Klient
{
    public class GetKlientQuery : IRequest<GetKlientResponse>
    {
        public int ID_osoba { get; set; }
    }

    public class GetKlientQueryHandle : IRequestHandler<GetKlientQuery, GetKlientResponse>
    {
        private readonly IKlientRepository repository;

        public GetKlientQueryHandle(IKlientRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<GetKlientResponse> Handle(GetKlientQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetKlientById(req.ID_osoba);
        }
    }
}