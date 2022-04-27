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
    public class GetKlientListQuery : IRequest<List<GetKlientListResponse>>
    {

    }

    public class GetKlientListQueryHandle : IRequestHandler<GetKlientListQuery, List<GetKlientListResponse>>
    {
        private readonly IKlientRepository repository;

        public GetKlientListQueryHandle(IKlientRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetKlientListResponse>> Handle(GetKlientListQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetKlientList();
        }
    }
}