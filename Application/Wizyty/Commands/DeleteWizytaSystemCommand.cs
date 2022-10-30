using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaSystemCommand : IRequest<int>
    {

    }

    public class DeleteWizytaSystemCommandHandler : IRequestHandler<DeleteWizytaSystemCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteWizytaSystemCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteWizytaSystemCommand req, CancellationToken cancellationToken)
        {
            var wizytaList = context.Wizyta
                .Where(x => x.Status.Equals(WizytaStatus.AnulowanaKlient.ToString()) || x.Status.Equals(WizytaStatus.AnulowanaKlinika.ToString()))
                .ToList();

            context.Wizyta.RemoveRange(wizytaList);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}