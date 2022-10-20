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
    public class CreateSpecjalizacjaCommand : IRequest<int>
    {
        public SpecjalizacjaRequest request { get; set; }
    }

    public class CreateSpecjalizacjaCommandHandle : IRequestHandler<CreateSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateSpecjalizacjaCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            context.Specjalizacjas.Add(
            new Specjalizacja
            {
                Nazwa = req.request.Nazwa,
                Opis = req.request.Opis
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}