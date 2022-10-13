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

namespace Application.Specjalizacje.Commands
{
    public class CreateSpecjalizacjaCommand : IRequest<string>
    {
        public SpecjalizacjaRequest request { get; set; }
    }

    public class CreateSpecjalizacjaCommandHandle : IRequestHandler<CreateSpecjalizacjaCommand, string>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateSpecjalizacjaCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<string> Handle(CreateSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            var result = await context.Specjalizacjas.AddAsync(
                new Specjalizacja
                {
                    Nazwa = req.request.Nazwa,
                    Opis = req.request.Opis
                });

            await context.SaveChangesAsync(cancellationToken);

            return hash.Encode(result.Entity.IdSpecjalizacja);
        }
    }
}