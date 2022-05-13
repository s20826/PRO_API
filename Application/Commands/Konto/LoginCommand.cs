using Application.DTO.Request;
using Application.DTO.Responses;
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
    public class LoginCommand : IRequest<LoginTokens>
    {
        public LoginRequest request { get; set; }
    }

    public class LoginCommandHandle : IRequestHandler<LoginCommand, LoginTokens>
    {
        private readonly IKontoRepository repository;

        public LoginCommandHandle(IKontoRepository kontoRepository)
        {
            repository = kontoRepository;
        }

        public async Task<LoginTokens> Handle(LoginCommand req, CancellationToken cancellationToken)
        {
            return await repository.Login(req.request);
        }
    }
}
