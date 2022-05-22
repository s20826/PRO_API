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
                (from x in context.Klients
                join y in context.Osobas on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where p.Rola == null
                orderby p.Nazwisko, p.Imie
                select new GetKlientListResponse()
                {
                    IdOsoba = hash.Encode(x.IdOsoba),
                    Imie = p.Imie,
                    Nazwisko = p.Nazwisko,
                    NumerTelefonu = p.NumerTelefonu,
                    Email = p.Email
                }).ToList();

            return results;
        }
    }
}