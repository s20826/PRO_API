using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Harmonogram
{
    public class HarmonogramWeterynarzQuery : IRequest<List<GetHarmonogramWeterynarzResponse>>
    {
        public DateTime Date { get; set; }
    }

    public class HarmonogramWeterynarzQueryHandle : IRequestHandler<HarmonogramWeterynarzQuery, List<GetHarmonogramWeterynarzResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramWeterynarzQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetHarmonogramWeterynarzResponse>> Handle(HarmonogramWeterynarzQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in context.Harmonograms
                 join k in context.Osobas on x.KlientIdOsoba equals k.IdOsoba
                 join p in context.Pacjents on x.IdPacjent equals p.IdPacjent
                 where x.DataRozpoczecia.Date == req.Date
                 select new GetHarmonogramWeterynarzResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdKlient = x.KlientIdOsoba != null ? hash.Encode((int)x.KlientIdOsoba) : null,
                     Data = x.DataRozpoczecia,
                     Klient = k.Imie + " " + k.Nazwisko,
                     IdPacjent = x.IdPacjent != null ? hash.Encode((int)x.IdPacjent) : null,
                     Pacjent = p.Nazwa,
                     CzyZajete = x.KlientIdOsoba != null
                 }).ToList();

            return results;
        }
    }
}
