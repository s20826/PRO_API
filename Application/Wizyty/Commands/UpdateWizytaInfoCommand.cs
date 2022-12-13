using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class UpdateWizytaInfoCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_weterynarz { get; set; }
        public WizytaInfoUpdateRequest request { get; set; }
    }

    public class UpdateWizytaInfoCommandHandle : IRequestHandler<UpdateWizytaInfoCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public UpdateWizytaInfoCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(UpdateWizytaInfoCommand req, CancellationToken cancellationToken)
        {
            (int wizytaID, int weterynarzID) = hash.Decode(req.ID_wizyta, req.ID_weterynarz);

            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(wizytaID)).FirstOrDefault();
            var harmonogram = context.Harmonograms.Where(x => x.IdWizyta.Equals(wizytaID)).FirstOrDefault();

            if (!harmonogram.WeterynarzIdOsoba.Equals(weterynarzID))
            {
                throw new UserNotAuthorizedException();
            }

            /*List<Usluga> uslugas = new List<Usluga>();

            for(int i = 0; i < req.request.Uslugi.Length; i++)
            {
                context.WizytaUslugas.Add(new WizytaUsluga
                {
                    IdUsluga = hash.Decode(req.request.Uslugi[i]),
                    IdWizyta = wizytaID
                });

                uslugas.Add(context.Uslugas.Where(x => x.IdUsluga.Equals(hash.Decode(req.request.Uslugi[i]))).First());
            }*/

            wizyta.Opis = req.request.Opis;
            wizyta.IdPacjent = req.request.ID_Pacjent != null ? hash.Decode(req.request.ID_Pacjent) : null;
            //wizyta.Cena = wizytaRepository.GetWizytaCena(uslugas);
            wizyta.Status = WizytaStatus.Zrealizowana.ToString();

            /*var klientZnizkaList = context.KlientZnizkas.Where(x => x.IdOsoba.Equals(wizyta.IdOsoba) && x.CzyWykorzystana == false).ToList();
            if (klientZnizkaList.Any())
            {
                var klientZnizka = klientZnizkaList.First();
                var znizka = context.Znizkas.Where(x => x.IdZnizka.Equals(klientZnizka.IdZnizka)).First();
                wizyta.IdZnizka = znizka.IdZnizka;
                wizyta.CenaZnizka = wizyta.Cena * (1 - (znizka.ProcentZnizki / 100));
                klientZnizka.CzyWykorzystana = true;
            }*/

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}