using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Weterynarz
{
    public class GetWeterynarzListQuery : IRequest<List<GetWeterynarzListResponse>>
    {

    }

    public class GetWeterynarzListQueryHandle : IRequestHandler<GetWeterynarzListQuery, List<GetWeterynarzListResponse>>
    {
        private readonly IWeterynarzRepository repository;

        public GetWeterynarzListQueryHandle(IWeterynarzRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetWeterynarzListResponse>> Handle(GetWeterynarzListQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetWeterynarzList();
        }
    }
}