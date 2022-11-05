using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Commands
{
    public class CreateUrlopCommand : IRequest<int>
    {
        public UrlopRequest request { get; set; }
    }

    public class CreateUrlopCommandHandler : IRequestHandler<CreateUrlopCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateUrlopCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateUrlopCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.request.ID_weterynarz);

            context.Urlops.Add(new Domain.Models.Urlop
            {
                IdOsoba = id,
                Dzien = req.request.Data
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}