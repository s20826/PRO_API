using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application.Queries.Lek
{
    public class GetLekListQuery : IRequest<List<GetLekListResponse>>
    {
       
    }

    public class GetLekListQueryHandle : IRequestHandler<GetLekListQuery, List<GetLekListResponse>>
    {
        private readonly ILekRepository repository;

        public GetLekListQueryHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetLekListResponse>> Handle(GetLekListQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetLekList();
        }
    }
}
