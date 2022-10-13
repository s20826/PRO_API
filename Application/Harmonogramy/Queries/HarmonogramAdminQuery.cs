using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminQuery : IRequest<List<GetHarmonogramAdminResponse>>
    {
        public string ID_osoba { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class HarmonogramAdminQueryHandle : IRequestHandler<HarmonogramAdminQuery, List<GetHarmonogramAdminResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramAdminQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetHarmonogramAdminResponse>> Handle(HarmonogramAdminQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var results =
                (from x in context.Harmonograms
                 join z in context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta from t in wizyta.DefaultIfEmpty()
                 join w in context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 where x.DataRozpoczecia.Date >= req.StartDate && x.DataZakonczenia.Date <= req.EndDate && x.WeterynarzIdOsoba == id
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdWeterynarz = hash.Encode(x.WeterynarzIdOsoba),
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Data = x.DataRozpoczecia,
                     IdKlient = t.IdOsoba != null ? hash.Encode((int)t.IdOsoba) : null,
                     Klient = t.IdOsoba != null ? context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
                     IdPacjent = t.IdPacjent != null ? hash.Encode((int)t.IdPacjent): null,
                     Pacjent = t.IdPacjent != null ? context.Pacjents.Where(p => p.IdPacjent == t.IdPacjent).Select(p => p.Nazwa).First() : null,
                     CzyZajete = x.IdWizyta != null
                 }).ToList();

            return results;
        }
    }
}
