using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
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
                 join z in context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta from t in wizyta.DefaultIfEmpty()
                 where x.DataRozpoczecia.Date == req.Date
                 select new GetHarmonogramWeterynarzResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     Data = x.DataRozpoczecia,
                     IdKlient = t.IdOsoba != null ? hash.Encode((int)t.IdOsoba) : null,
                     Klient = t.IdOsoba != null ? context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
                     IdPacjent = t.IdPacjent != null ? hash.Encode((int)t.IdPacjent) : null,
                     Pacjent = t.IdPacjent != null ? context.Pacjents.Where(p => p.IdPacjent == t.IdPacjent).Select(p => p.Nazwa).First() : null,
                     CzyZajete = t.IdOsoba != null
                 }).ToList();

            return results;
        }
    }
}