using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.GodzinyPracy
{
    public class DeleteGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public string request { get; set; }
    }

    public class DeleteGodzinyPracyCommandHandle : IRequestHandler<DeleteGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);
            var i = 0;
            var list = context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (list.Any())
            {
                throw new Exception("Ten pracownik nie ma ustawionych godzin pracy.");
            }

            context.GodzinyPracies.Remove(context.GodzinyPracies.Where(x => ((DniTygodnia)x.DzienTygodnia).ToString() == req.request).FirstOrDefault());

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
