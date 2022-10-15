using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaKlientQuery : IRequest<List<GetWizytaListResponse>>
    {
        public string ID_klient { get; set; }
    }

    public class WizytaKlientQueryHandle : IRequestHandler<WizytaKlientQuery, List<GetWizytaListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaKlientQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetWizytaListResponse>> Handle(WizytaKlientQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_klient);

            var results =
                (from x in context.Wizyta
                 join y in context.Harmonograms on x.IdWizyta equals y.IdWizyta
                 join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                 join w in context.Osobas on y.WeterynarzIdOsoba equals w.IdOsoba
                 join p in context.Pacjents on x.IdPacjent equals p.IdPacjent
                 where k.IdOsoba == id
                 orderby y.DataRozpoczecia descending
                 select new GetWizytaListResponse()
                 {
                     IdWizyta = hash.Encode(x.IdWizyta),
                     IdKlient = req.ID_klient,
                     IdWeterynarz = null,
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