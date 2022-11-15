using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class UpdateChorobaCommand : IRequest<int>
    {
        public string ID_Choroba { get; set; }
        public ChorobaRequest request { get; set; }
    }

    public class UpdateChorobaCommandHandler : IRequestHandler<UpdateChorobaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateChorobaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateChorobaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Choroba);

            var choroba = context.Chorobas.Where(x => x.IdChoroba.Equals(id)).First();
            choroba.Nazwa = req.request.Nazwa;
            choroba.NazwaLacinska = req.request.NazwaLacinska;
            choroba.Opis = req.request.Opis;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}