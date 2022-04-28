using Application.DTO;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Klient
{
    public class DeleteKlientCommand : IRequest<int>
    {
        public int ID_osoba { get; set; }
    }

    public class DeleteKlientCommandHandle : IRequestHandler<DeleteKlientCommand, int>
    {
        private readonly IKlientRepository repository;

        public DeleteKlientCommandHandle(IKlientRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(DeleteKlientCommand req, CancellationToken cancellationToken)
        {
            return await repository.DeleteKlient(req.ID_osoba);
        }
    }
}