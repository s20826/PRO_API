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
        private readonly IKlinikaContext context;

        public GetPacjentListQueryHandle(IKlinikaContext klinikaContext)
        {
            context = klinikaContext;
        }

        public async Task<List<GetPacjentListResponse>> Handle(GetPacjentListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Pacjents
                    join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                    orderby x.Nazwa
                    select new GetPacjentListResponse()
                    {
                        IdOsoba = x.IdOsoba,
                        IdPacjent = x.IdPacjent,
                        Nazwa = x.Nazwa,
                        Gatunek = x.Gatunek,
                        Rasa = x.Rasa,
                        Masc = x.Masc,
                        Wlasciciel = y.Imie + ' ' + y.Nazwisko
                    }).ToList();
        }
    }
}
