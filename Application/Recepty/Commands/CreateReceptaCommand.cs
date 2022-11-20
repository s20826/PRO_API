using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Commands
{
    public class CreateReceptaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string Zalecenia { get; set; }
    }

    public class ReceptaDetailsQueryHandler : IRequestHandler<CreateReceptaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public ReceptaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            context.Recepta.Add(new Domain.Models.Receptum
            {
                IdWizyta = id,
                Zalecenia = req.Zalecenia
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}