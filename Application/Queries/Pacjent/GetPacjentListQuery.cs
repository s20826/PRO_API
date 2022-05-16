using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Pacjent
{
    public class GetPacjentListQuery : IRequest<List<GetPacjentListResponse>>
    {

    }

    public class GetPacjentListQueryHandle : IRequestHandler<GetPacjentListQuery, List<GetPacjentListResponse>>
    {
        private readonly IPacjentRepository repository;

        public GetPacjentListQueryHandle(IPacjentRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetPacjentListResponse>> Handle(GetPacjentListQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetPacjentList();
        }
    }
}
