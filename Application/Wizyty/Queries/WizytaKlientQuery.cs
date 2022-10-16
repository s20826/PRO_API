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
                 join z in context.Harmonograms on x.IdWizyta equals z.IdWizyta into harmonogram from y in harmonogram.DefaultIfEmpty()
                 join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                 join d in context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent from p in pacjent.DefaultIfEmpty()
                 where k.IdOsoba == id
                 select new GetWizytaListResponse()
                 {
                     IdWizyta = hash.Encode(x.IdWizyta),
                     IdKlient = req.ID_klient,
                     IdWeterynarz = null,
                     Status = x.Status,
                     Data = y.IdWizyta != null ? y.DataRozpoczecia : null,
                     CzyOplacona = x.CzyOplacona,
                     Weterynarz = y.IdWizyta != null ? context.Osobas.Where(i => i.IdOsoba == y.WeterynarzIdOsoba).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                     Klient = k.Imie + " " + k.Nazwisko,
                     IdPacjent = x.IdPacjent != null ? hash.Encode(p.IdPacjent) : null,
                     Pacjent = x.IdPacjent != null ? p.Nazwa : null
                 }).ToList().OrderByDescending(x => x.Data).ToList();

            return results;
        }
    }
}