using Application.Interfaces;
using Domain.Enums;
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
        public DeleteWizytaAdminCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(DeleteWizytaAdminCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            var harmonogram = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();
            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            harmonogram.IdWizyta = null;
            wizyta.Status = WizytaStatus.AnulowanaKlinika.ToString();

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}