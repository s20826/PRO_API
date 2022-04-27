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
    public class RefreshCommand : IRequest<string>
    {
        public Guid RefreshToken { get; set; }
    }

    public class RefreshCommandHandle : IRequestHandler<RefreshCommand, string>
    {
        private readonly IKontoRepository repository;

        public RefreshCommandHandle(IKontoRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<string> Handle(RefreshCommand req, CancellationToken cancellationToken)
        {
            return await repository.GetToken(req.RefreshToken);
        }
    }
}