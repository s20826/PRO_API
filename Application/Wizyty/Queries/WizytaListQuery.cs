﻿using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaListQuery : IRequest<List<GetWizytaListResponse>>
    {

    }

    public class WizytaListQueryHandle : IRequestHandler<WizytaListQuery, List<GetWizytaListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaListQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetWizytaListResponse>> Handle(WizytaListQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in context.Wizyta
                 join y in context.Harmonograms on x.IdHarmonogram equals y.IdHarmonogram
                 join k in context.Osobas on y.KlientIdOsoba equals k.IdOsoba
                 join w in context.Osobas on y.WeterynarzIdOsoba equals w.IdOsoba
                 join p in context.Pacjents on y.IdPacjent equals p.IdPacjent
                 orderby y.DataRozpoczecia
                 select new GetWizytaListResponse()
                 {
                     IdWizyta = hash.Encode(x.IdWizyta),
                     IdKlient = y.KlientIdOsoba != null ? hash.Encode((int)y.KlientIdOsoba) : "",
                     IdWeterynarz = hash.Encode(y.WeterynarzIdOsoba),
                     Status = x.Status,
                     Data = y.DataRozpoczecia,
                     CzyOplacona = x.CzyOplacona,
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Klient = k.Imie + " " + k.Nazwisko,
                     IdPacjent = y.IdPacjent != null ? hash.Encode((int)y.IdPacjent) : "",
                     Pacjent = p.Nazwa
                 }).ToList();

            return results;
        }
    }
}