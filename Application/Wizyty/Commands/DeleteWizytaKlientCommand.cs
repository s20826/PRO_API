﻿using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaKlientCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class DeleteWizytaKlientCommandHandle : IRequestHandler<DeleteWizytaKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public DeleteWizytaKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(DeleteWizytaKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            var harmonogram = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();
            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!wizytaRepository.IsWizytaAbleToCancel(harmonogram.DataRozpoczecia)){
                //naliczenie kary lub wysłanie powiadomienia
            }

            harmonogram.IdWizyta = null;
            wizyta.Status = WizytaStatus.AnulowanaKlient.ToString();

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}