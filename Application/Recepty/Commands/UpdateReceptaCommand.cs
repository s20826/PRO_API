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
    public class UpdateReceptaCommand : IRequest<int>
    {
        public string ID_recepta { get; set; }
        public string Zalecenia { get; set; }
    }

    public class UpdateReceptaCommandHandler : IRequestHandler<UpdateReceptaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateReceptaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_recepta);

            var recepta = context.Recepta.Where(x => x.IdWizyta.Equals(req.ID_recepta)).First();
            recepta.Zalecenia = req.Zalecenia;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}