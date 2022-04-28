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
    public class CreateKlientCommand : IRequest<int>
    {
        public KlientCreateRequest request { get; set; }
    }

    public class CreateKlientCommandHandle : IRequestHandler<CreateKlientCommand, int>
    {
        private readonly IKlientRepository repository;

        public CreateKlientCommandHandle(IKlientRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(CreateKlientCommand req, CancellationToken cancellationToken)
        {
            return await repository.AddKlient(req.request);
        }
    }
}