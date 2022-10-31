using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaAdminCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class DeleteWizytaAdminCommandHandle : IRequestHandler<DeleteWizytaAdminCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        private readonly IEmailSender sender;
        public DeleteWizytaAdminCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository, IEmailSender emailSender)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
            sender = emailSender;
        }

        public async Task<int> Handle(DeleteWizytaAdminCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).ToList();
            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!harmonograms.Any())
            {
                throw new NotFoundException();
            }

            foreach (Harmonogram h in harmonograms)
            {
                h.IdWizyta = null;
            }

            wizyta.Status = WizytaStatus.AnulowanaKlinika.ToString();
            await context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem anulowania wizyty do klienta
            var to = context.Osobas.Where(x => x.IdOsoba.Equals(wizyta.IdOsoba)).First().Email;
            //await sender.SendWizytaEmail(to, harmonogram.DataRozpoczecia.ToString());

            return 0;
        }
    }
}