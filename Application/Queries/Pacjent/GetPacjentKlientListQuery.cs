using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Pacjent
{
    public class GetPacjentKlientListQuery : IRequest<List<GetPacjentKlientListResponse>>
    {
        public int ID_osoba { get; set; }
    }

    public class GetPacjentKlientListQueryHandle : IRequestHandler<GetPacjentKlientListQuery, List<GetPacjentKlientListResponse>>
    {
        private readonly IKlinikaContext context;

        public GetPacjentKlientListQueryHandle(IKlinikaContext klinikaContext)
        {
            context = klinikaContext;
        }

        public async Task<List<GetPacjentKlientListResponse>> Handle(GetPacjentKlientListQuery req, CancellationToken cancellationToken)
        {
            if (context.Klients.Where(x => x.IdOsoba == req.ID_osoba).Any() != true)
            {
                throw new Exception("Nie ma klienta o ID = " + req.ID_osoba);
            }

            var results =
            (from x in context.Pacjents
             where x.IdOsoba == req.ID_osoba
             orderby x.Nazwa
             select new GetPacjentKlientListResponse()
             {
                 IdPacjent = x.IdPacjent,
                 Nazwa = x.Nazwa,
                 Gatunek = x.Gatunek,
                 Rasa = x.Rasa,
                 Masc = x.Masc
             }).ToList();

            return results;
        }
    }
}