using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class DeletePacjentCommand : IRequest<int>
    {
        public string ID_Pacjent { get; set; }
    }

    public class DeletePacjentCommandHandle : IRequestHandler<DeletePacjentCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeletePacjentCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeletePacjentCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Pacjent);

            var pacjent = context.Pacjents.Where(x => x.IdPacjent == id).First();

            context.Pacjents.Remove(pacjent);
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}