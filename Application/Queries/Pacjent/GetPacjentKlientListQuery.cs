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
    public class GetPacjentKlientListQuery : IRequest<List<GetPacjentKlientListResponse>>
    {
        public int ID_osoba { get; set; }
    }

    public class GetPacjentKlientListQueryHandle : IRequestHandler<GetPacjentKlientListQuery, List<GetPacjentKlientListResponse>>
    {
        private readonly IPacjentRepository repository;

        public GetPacjentKlientListQueryHandle(IPacjentRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<List<GetPacjentKlientListResponse>> Handle(GetPacjentKlientListQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetPacjentList(req.ID_osoba);
        }
    }
}