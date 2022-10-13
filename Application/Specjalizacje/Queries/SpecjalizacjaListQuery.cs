using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Queries
{
    public class SpecjalizacjaListQuery : IRequest<List<GetSpecjalizacjaResponse>>
    {

    }

    public class SpecjalizacjaListQueryHandle : IRequestHandler<SpecjalizacjaListQuery, List<GetSpecjalizacjaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaListQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetSpecjalizacjaResponse>> Handle(SpecjalizacjaListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Specjalizacjas
                    orderby x.Nazwa
                    select new GetSpecjalizacjaResponse()
                    {
                        IdSpecjalizacja = hash.Encode(x.IdSpecjalizacja),
                        Nazwa = x.Nazwa,
                        Opis = x.Opis
                    }).ToList();
        }
    }
}