using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class CreateChorobaCommand : IRequest<int>
    {
        public ChorobaRequest request { get; set; }
    }

    public class CreateChorobaCommandHandler : IRequestHandler<CreateChorobaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateChorobaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateChorobaCommand req, CancellationToken cancellationToken)
        {
            context.Chorobas.Add(new Choroba
            {
                Nazwa = req.request.Nazwa
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}