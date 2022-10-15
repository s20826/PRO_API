using Application.DTO.Responses;
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
    public class WizytaWeterynarzQuery : IRequest<List<GetWizytaListResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class WizytaWeterynarzQueryHandle : IRequestHandler<WizytaWeterynarzQuery, List<GetWizytaListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaWeterynarzQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetWizytaListResponse>> Handle(WizytaWeterynarzQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_weterynarz);

            var results =
                (from x in context.Wizyta
                 join y in context.Harmonograms on x.IdWizyta equals y.IdHarmonogram
                 join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                 join w in context.Osobas on y.WeterynarzIdOsoba equals w.IdOsoba
                 join p in context.Pacjents on x.IdPacjent equals p.IdPacjent
                 where w.IdOsoba == id
                 select new GetWizytaListResponse()
                 {
                     IdWizyta = hash.Encode(x.IdWizyta),
                     IdKlient = hash.Encode(k.IdOsoba),
                     IdWeterynarz = req.ID_weterynarz,
                     Status = x.Status,
                     Data = y.DataRozpoczecia,
                     CzyOplacona = x.CzyOplacona,
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Klient = k.Imie + " " + k.Nazwisko,
                     IdPacjent = hash.Encode(p.IdPacjent),
                     Pacjent = p.Nazwa
                 }).ToList();

            return results;
        }
    }
}