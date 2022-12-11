﻿using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
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

    public class ReceptaKlientQueryHandler : IRequestHandler<ReceptaKlientQuery, List<GetReceptaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public ReceptaKlientQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetReceptaResponse>> Handle(ReceptaKlientQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_klient);

            return (from x in context.Recepta
                    join s in context.Wizyta on x.IdWizyta equals s.IdWizyta
                    join l in context.ReceptaLeks on x.IdWizyta equals l.IdWizyta into receptaLek
                    from y in receptaLek.DefaultIfEmpty()
                    where s.IdOsoba == id
                    select new GetReceptaResponse()
                    {
                        ID_Recepta = hash.Encode(x.IdWizyta),
                        Zalecenia = x.Zalecenia,
                        Leki = x.ReceptaLeks.Select(x => new GetReceptaLekResponse
                        {
                            ID_Lek = hash.Encode(x.IdLek),
                            Nazwa = x.IdLekNavigation.Nazwa,
                            Ilosc = y.Ilosc,
                            Producent = x.IdLekNavigation.Producent,
                            JednostkaMiary = x.IdLekNavigation.JednostkaMiary
                        }).ToList(),
                        WizytaData = context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Any() ? context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Min(y => y.DataRozpoczecia) : null,
                    }).ToList();
        }
    }
}