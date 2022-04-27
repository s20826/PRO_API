using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Konto
{
    public class UpdateKontoCommand : IRequest<int>
    {
        public int ID_osoba { get; set; }
        public KontoUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateKontoCommand, int>
    {
        private readonly IKontoRepository repository;

        public UpdateKontoCommandHandle(IKontoRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            return await repository.UpdateKontoCredentials(req.ID_osoba, req.request);
        }
    }
}