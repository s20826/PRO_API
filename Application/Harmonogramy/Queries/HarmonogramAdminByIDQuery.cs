﻿using Application.DTO.Responses;
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
    public class HarmonogramAdminByIDQuery : IRequest<List<GetHarmonogramAdminResponse>>
    {
        public string ID_osoba { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class HarmonogramAdminByIDQueryHandler : IRequestHandler<HarmonogramAdminByIDQuery, List<GetHarmonogramAdminResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramAdminByIDQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetHarmonogramAdminResponse>> Handle(HarmonogramAdminByIDQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var results =
                (from x in context.Harmonograms
                 join z in context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta
                 from t in wizyta.DefaultIfEmpty()
                 join w in context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 where x.DataRozpoczecia.Date >= req.StartDate && x.DataZakonczenia.Date <= req.EndDate && x.WeterynarzIdOsoba == id
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdWeterynarz = req.ID_osoba,
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Data = x.DataRozpoczecia,
                     IdKlient = x.IdWizyta != null ? hash.Encode(t.IdOsoba) : null,
                     Klient = x.IdWizyta != null ? context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
                     IdPacjent = x.IdWizyta != null ? hash.Encode((int)t.IdPacjent) : null,
                     Pacjent = x.IdWizyta != null ? context.Pacjents.Where(p => p.IdPacjent == t.IdPacjent).Select(p => p.Nazwa).First() : null,
                     CzyZajete = x.IdWizyta != null
                 }).ToList();

            return results;
        }
    }
}
