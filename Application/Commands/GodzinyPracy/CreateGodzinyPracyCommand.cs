﻿using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Commands.GodzinyPracy
{
    public class CreateGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public List<GodzinyPracyRequest> requestList { get; set; }
    }

    public class CreateGodzinyPracyCommandHandle : IRequestHandler<CreateGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);
            var i = 0;
            var list = context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (list.Any())
            {
                throw new Exception("Ten pracownik ma już ustawione godziny pracy.");
            }
            foreach (GodzinyPracyRequest request in req.requestList)
            {
                var dzien = (DniTygodnia)Enum.Parse(typeof(DniTygodnia), request.DzienTygodnia, true);
                i = (int)dzien;
                context.GodzinyPracies.Add(new Models.GodzinyPracy
                {
                    IdOsoba = id,
                    DzienTygodnia = (int)dzien,
                    GodzinaRozpoczecia = request.GodzinaRozpoczecia,
                    GodzinaZakonczenia = request.GodzinaZakonczenia
                });
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}