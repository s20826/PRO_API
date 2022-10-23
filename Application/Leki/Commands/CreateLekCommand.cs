using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class CreateLekCommand : IRequest<int>
    {
        public LekRequest request { get; set; }
    }

    public class CreateLekCommandHandler : IRequestHandler<CreateLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateLekCommand req, CancellationToken cancellationToken)
        {
            context.Leks.Add(new Lek
            {
                Nazwa = req.request.Nazwa,
                JednostkaMiary = req.request.JednostkaMiary
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}