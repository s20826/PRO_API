﻿using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class DeleteChorobaCommand : IRequest<int>
    {
        public string ID_Choroba { get; set; }
    }

    public class DeleteChorobaCommandHandler : IRequestHandler<DeleteChorobaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteChorobaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteChorobaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Choroba);

            context.ChorobaLeks.RemoveRange(context.ChorobaLeks.Where(x => x.IdChoroba.Equals(id)).ToList());
            context.Chorobas.Remove(context.Chorobas.Where(x => x.IdChoroba.Equals(id)).First());

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}