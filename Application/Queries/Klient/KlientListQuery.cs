using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Klient
{
    public class KlientListQuery : IRequest<List<GetKlientListResponse>>
    {

    }

    public class GetKlientListQueryHandle : IRequestHandler<KlientListQuery, List<GetKlientListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetKlientListQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetKlientListResponse>> Handle(KlientListQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                orderby x.Nazwisko, x.Imie
                select new GetKlientListResponse()
                {
                    IdOsoba = hash.Encode(x.IdOsoba),
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZalozeniaKonta = p.DataZalozeniaKonta
                }).ToList();

            return results;
        }
    }
}