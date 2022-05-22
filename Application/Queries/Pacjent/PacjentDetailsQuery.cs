using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Queries.Pacjent
{
    public class PacjentDetailsQuery : IRequest<GetPacjentDetailsResponse>
    {
        public string ID_pacjent { get; set; }
    }

    public class PacjentDetailsQueryHandle : IRequestHandler<PacjentDetailsQuery, GetPacjentDetailsResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public PacjentDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetPacjentDetailsResponse> Handle(PacjentDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_pacjent);

            if (context.Pacjents.Where(x => x.IdPacjent == id).Any() != true)
            {
                throw new Exception("Nie ma pacjenta o ID = " + req.ID_pacjent);
            }
            
            var results =
                (from x in context.Pacjents
                 join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                 where x.IdPacjent == id
                 orderby x.Nazwa
                 select new GetPacjentDetailsResponse()
                 {
                     IdOsoba = hash.Encode(x.IdOsoba),
                     Nazwa = x.Nazwa,
                     Gatunek = x.Gatunek,
                     Rasa = x.Rasa,
                     Masc = x.Masc,
                     Plec = x.Plec,
                     DataUrodzenia = x.DataUrodzenia,
                     Waga = x.Waga,
                     Agresywne = x.Agresywne,
                     Wlasciciel = y.Imie + " " + y.Nazwisko,
                     Wizyty = new PacjentWizytaResponse[0]

                 }).First();

            return results;
        }
    }
}
