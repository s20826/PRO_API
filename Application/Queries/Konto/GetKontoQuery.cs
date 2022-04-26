using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Konto
{
    public class GetKontoQuery : IRequest<GetKontoResponse>
    {
        public int ID_osoba { get; set; }
    }

    public class GetKontoQueryHandle : IRequestHandler<GetKontoQuery, GetKontoResponse>
    {
        private readonly IKontoRepository repository;

        public GetKontoQueryHandle(IKontoRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<GetKontoResponse> Handle(GetKontoQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetKonto(req.ID_osoba);
        }
    }
}