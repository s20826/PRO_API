using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PacjentListQuery : IRequest<List<GetPacjentListResponse>>
    {

    }

    public class GetPacjentListQueryHandle : IRequestHandler<PacjentListQuery, List<GetPacjentListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetPacjentListQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetPacjentListResponse>> Handle(PacjentListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Pacjents
                    join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                    orderby x.Nazwa
                    select new GetPacjentListResponse()
                    {
                        IdOsoba = hash.Encode(x.IdOsoba),
                        IdPacjent = hash.Encode(x.IdPacjent),
                        Nazwa = x.Nazwa,
                        Gatunek = x.Gatunek,
                        Rasa = x.Rasa,
                        Plec = x.Plec,
                        Wlasciciel = y.Imie + ' ' + y.Nazwisko
                    }).ToList();
        }
    }
}
