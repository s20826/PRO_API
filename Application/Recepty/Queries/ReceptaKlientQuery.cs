using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Queries
{
    public class ReceptaKlientQuery : IRequest<List<GetReceptaResponse>>
    {
        public string ID_klient { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandle : IRequestHandler<ReceptaKlientQuery, List<GetReceptaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetReceptaResponse>> Handle(ReceptaKlientQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_klient);

            return (from x in context.Recepta
                    join s in context.Wizyta on x.IdWizyta equals s.IdWizyta
                    where s.IdOsoba == id
                    select new GetReceptaResponse()
                    {
                        ID_Recepta = hash.Encode(x.IdWizyta),
                        Zalecenia = x.Zalecenia,
                        WizytaData = context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Any() ? context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Min(y => y.DataRozpoczecia) : null,
                    }).ToList();
        }
    }
}