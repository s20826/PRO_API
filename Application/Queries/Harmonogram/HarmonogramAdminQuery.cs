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
                 join k in context.Osobas on x.KlientIdOsoba equals k.IdOsoba
                 join w in context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 join p in context.Pacjents on x.IdPacjent equals p.IdPacjent
                 where x.DataRozpoczecia.Date >= req.StartDate && x.DataZakonczenia.Date <= req.EndDate && x.WeterynarzIdOsoba == id
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdWeterynarz = hash.Encode(x.WeterynarzIdOsoba),
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Data = x.DataRozpoczecia,
                     IdKlient = x.KlientIdOsoba != null ? hash.Encode((int)x.KlientIdOsoba) : null,
                     Klient = k.Imie + " " + k.Nazwisko,
                     IdPacjent = x.IdPacjent != null ? hash.Encode((int)x.IdPacjent) : null,
                     Pacjent = p.Nazwa,
                     CzyZajete = x.KlientIdOsoba != null
                 }).ToList();

            return results;
        }
    }
}
