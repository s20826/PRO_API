﻿using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
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
            if(context.Chorobas.Where(x => x.Nazwa.Equals(req.request.Nazwa)).Any())
            {
                throw new Exception("already exists");
            }

            context.Chorobas.Add(new Choroba
            {
                Nazwa = req.request.Nazwa,
                NazwaLacinska = req.request.NazwaLacinska,
                Opis = req.request.Opis
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}