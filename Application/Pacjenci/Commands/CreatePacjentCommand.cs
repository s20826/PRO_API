using Application.DTO.Request;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class CreatePacjentCommand : IRequest<int>
    {
        public PacjentCreateRequest request { get; set; }
    }

    public class CreatePacjentCommandHandle : IRequestHandler<CreatePacjentCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreatePacjentCommandHandle(IKlinikaContext klinikaContext, IHash ihash)
        {
            context = klinikaContext;
            hash = ihash;
        }

        public async Task<int> Handle(CreatePacjentCommand req, CancellationToken cancellationToken)
        {
            context.Pacjents.Add(new Pacjent
            {
                IdOsoba = hash.Decode(req.request.IdOsoba),
                Nazwa = req.request.Nazwa,
                Gatunek = req.request.Gatunek,
                Rasa = req.request.Rasa,
                Waga = req.request.Waga,
                Masc = req.request.Masc,
                DataUrodzenia = req.request.DataUrodzenia,
                Plec = req.request.Plec,
                Agresywne = req.request.Agresywne,
                Ubezplodnienie = req.request.Ubezplodnienie
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}