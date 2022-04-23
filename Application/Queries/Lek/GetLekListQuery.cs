using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Lek
{
    public class GetLekListQuery : IRequest<GetLekListResponse>
    {
       
    }

    public class GetLekListQueryHandle : IRequestHandler<GetLekListQuery, GetLekListResponse>
    {
        private ILekRepository repository;

        public GetLekListQueryHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<GetLekListResponse> Handle(GetLekListQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetLekList();
        }
    }
}
