using Application.DTO;
using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Weterynarz
{
    public class UpdateWeterynarzCommand : IRequest<int>
    {
        public int ID_osoba { get; set; }
        public WeterynarzUpdateRequest request { get; set; }
    }

    public class UpdateWeterynarzCommandHandle : IRequestHandler<UpdateWeterynarzCommand, int>
    {
        private readonly IWeterynarzRepository repository;

        public UpdateWeterynarzCommandHandle(IWeterynarzRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(UpdateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            return await repository.UpdateWeterynarzZatrudnienie(req.ID_osoba, req.request);
        }
    }
}