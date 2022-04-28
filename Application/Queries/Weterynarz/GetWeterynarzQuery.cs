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
    public class GetWeterynarzQuery : IRequest<GetWeterynarzResponse>
    {
        public int ID_osoba { get; set; }
    }

    public class GetWeterynarzQueryHandle : IRequestHandler<GetWeterynarzQuery, GetWeterynarzResponse>
    {
        private readonly IWeterynarzRepository repository;

        public GetWeterynarzQueryHandle(IWeterynarzRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<GetWeterynarzResponse> Handle(GetWeterynarzQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetWeterynarzById(req.ID_osoba);
        }
    }
}