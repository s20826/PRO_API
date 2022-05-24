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
    public class UpdateGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public List<GodzinyPracyRequest> requestList { get; set; }
    }

    public class UpdateGodzinyPracyCommandHandle : IRequestHandler<UpdateGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);
            var list = context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (!list.Any())
            {
                throw new Exception("Ten pracownik nie ma ustawionych godzin pracy.");
            }
            foreach (GodzinyPracyRequest request in req.requestList)
            {
                var dzien = list.Where(x => ((DniTygodnia)x.DzienTygodnia).ToString() == request.DzienTygodnia).FirstOrDefault();
                dzien.GodzinaRozpoczecia = request.GodzinaRozpoczecia;
                dzien.GodzinaZakonczenia = request.GodzinaZakonczenia;
            } 

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}