using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class UpdatePacjentCommand : IRequest<int>
    {
        public PacjentCreateRequest request { get; set; }
        public string ID_pacjent { get; set; }
    }

    public class UpdatePacjentCommandHandle : IRequestHandler<UpdatePacjentCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdatePacjentCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdatePacjentCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_pacjent);

            var pacjent = context.Pacjents.Where(x => x.IdPacjent == id).FirstOrDefault();
            if (pacjent is null)
            {
                throw new Exception();
            }

            pacjent.IdOsoba = req.request.IdOsoba;
            pacjent.Nazwa = req.request.Nazwa;
            pacjent.Gatunek = req.request.Gatunek;
            pacjent.Rasa = req.request.Rasa;
            pacjent.Masc = req.request.Masc;
            pacjent.Plec = req.request.Plec;
            pacjent.DataUrodzenia = req.request.DataUrodzenia;
            pacjent.Waga = req.request.Waga;
            pacjent.Agresywne = req.request.Agresywne;
            pacjent.Ubezplodnienie = req.request.Ubezplodnienie;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}