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
    public class ReceptaDetailsQuery : IRequest<GetReceptaResponse>
    {
        public string ID_recepta { get; set; }
    }

    public class ReceptaDetailsQueryHandler : IRequestHandler<ReceptaDetailsQuery, GetReceptaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public ReceptaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetReceptaResponse> Handle(ReceptaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_recepta);

            return (from x in context.Recepta
                    join s in context.Wizyta on x.IdWizyta equals s.IdWizyta
                    where s.IdWizyta == id
                    select new GetReceptaResponse()
                    {
                        ID_Recepta = hash.Encode(x.IdWizyta),
                        Zalecenia = x.Zalecenia,
                        WizytaData = context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Any() ? context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Min(y => y.DataRozpoczecia) : null
                    }).First();
        }
    }
}