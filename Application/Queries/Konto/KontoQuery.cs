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
    public class KontoQuery : IRequest<GetKontoResponse>
    {
        public int ID_osoba { get; set; }
    }

    public class GetKontoQueryHandle : IRequestHandler<KontoQuery, GetKontoResponse>
    {
        private readonly IKlinikaContext context;

        public GetKontoQueryHandle(IKlinikaContext klinikaContext)
        {
            context = klinikaContext;
        }

        public async Task<GetKontoResponse> Handle(KontoQuery req, CancellationToken cancellationToken)
        {
            return context.Osobas.Where(x => x.IdOsoba == req.ID_osoba).Select(x => new GetKontoResponse()
            {
                Imie = x.Imie,
                Nazwisko = x.Nazwisko,
                NumerTelefonu = x.NumerTelefonu,
                Email = x.Email
            }).FirstOrDefault();
        }
    }
}