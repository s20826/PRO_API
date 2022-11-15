using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Queries
{
    public class ChorobaListQuery : IRequest<List<GetChorobaResponse>>
    {

    }

    public class UslugaListQueryHandler : IRequestHandler<ChorobaListQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UslugaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetChorobaResponse>> Handle(ChorobaListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Chorobas
                    orderby x.Nazwa
                    select new GetChorobaResponse()
                    {
                        ID_Choroba = hash.Encode(x.IdChoroba),
                        Nazwa = x.Nazwa,
                        NazwaLacinska = x.NazwaLacinska,
                        Opis = x.Opis
                    }).ToList();
        }
    }
}