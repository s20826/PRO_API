using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Lek
{
    public class StanLekuQuery : IRequest<GetStanLekuResponse>
    {
        public string ID_stan_leku { get; set; }
    }

    public class GetStanLekuQueryHandle : IRequestHandler<StanLekuQuery, GetStanLekuResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetStanLekuQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetStanLekuResponse> Handle(StanLekuQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_stan_leku);

            var result =
            (from x in context.Leks
             join y in context.LekWMagazynies on x.IdLek equals y.IdLek into ps
             from p in ps
             where p.IdStanLeku == id
             select new GetStanLekuResponse()
             {
                 IdStanLeku = hash.Encode(p.IdStanLeku),
                 Nazwa = x.Nazwa,
                 JednostkaMiary = x.JednostkaMiary,
                 Ilosc = (uint)p.Ilosc,
                 DataWaznosci = p.DataWaznosci
             }).FirstOrDefault();

            return result;
        }
    }
}

