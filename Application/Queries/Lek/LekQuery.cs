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
    public class LekQuery : IRequest<List<GetLekResponse>>
    {
        public string ID_lek { get; set; }
    }

    public class GetLekQueryHandle : IRequestHandler<LekQuery, List<GetLekResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetLekQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetLekResponse>> Handle(LekQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            var results = (from x in context.Leks
                           join y in context.LekWMagazynies on x.IdLek equals y.IdLek
                           where x.IdLek == id
                           select new GetLekResponse()
                           {
                               IdStanLeku = hash.Encode(y.IdStanLeku),
                               Nazwa = x.Nazwa,
                               JednostkaMiary = x.JednostkaMiary,
                               Ilosc = (uint)y.Ilosc,
                               DataWaznosci = y.DataWaznosci,
                               Choroby = (from i in context.ChorobaLeks
                                          join j in context.Chorobas on i.IdChoroba equals j.IdChoroba into qs
                                          from j in qs.DefaultIfEmpty()
                                          where i.IdLek == x.IdLek
                                          select new
                                          {
                                              j.Nazwa
                                          }).ToArray()
                           }).ToList();

            return results;
        }
    }
}
