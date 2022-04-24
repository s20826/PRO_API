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
    public class GetLekQuery : IRequest<List<GetLekResponse>>
    {
        public int ID_lek { get; set; }
    }

    public class GetLekQueryHandle : IRequestHandler<GetLekQuery, List<GetLekResponse>>
    {
        private readonly ILekRepository repository;

        public GetLekQueryHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetLekResponse>> Handle(GetLekQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetLekById(request.ID_lek);
        }
    }
}
