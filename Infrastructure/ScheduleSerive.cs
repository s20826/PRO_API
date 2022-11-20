using Application.Interfaces;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ScheduleSerive : ISchedule
    {
        private readonly ILogger<ScheduleSerive> logger;
        private readonly IKlinikaContext context;
        private readonly IEmailSender emailSender;
        public ScheduleSerive(ILogger<ScheduleSerive> log, IKlinikaContext klinikaContext, IEmailSender sender)
        {
            logger = log;
            context = klinikaContext;
            emailSender = sender;
        }

        public void SendPrzypomnienieEmail()
        {
            var helperList = from x in context.Wizyta
                             join y in context.Harmonograms on x.IdWizyta equals y.IdWizyta
                             join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                             where x.Status == WizytaStatus.Zaplanowana.ToString()
                             group x by new { x.IdWizyta, x.IdOsoba, y.WeterynarzIdOsoba, k.Email }
                              into g
                             select new
                             {
                                 Email = g.Key.Email,
                                 Weterynarz = g.Key.WeterynarzIdOsoba != 0 ? context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                                 Data = context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).Min(x => x.DataRozpoczecia)
                             };

            foreach (var a in helperList)
            {
                emailSender.SendPrzypomnienieEmail(a.Email, a.Data, a.Weterynarz);
                logger.LogInformation("Email sent to " + a.Email + " at: " + DateTime.Now.ToString());
            }
        }

        public async Task DeleteWizytaSystemAsync()
        {
            var wizytaList = context.Wizyta
               .Where(x => x.Status.Equals(WizytaStatus.AnulowanaKlient.ToString()) || x.Status.Equals(WizytaStatus.AnulowanaKlinika.ToString()))
               .ToList();

            context.Wizyta.RemoveRange(wizytaList);

            var result = await context.SaveChangesAsync(new CancellationToken());
            logger.LogInformation("Records deleted: " + result);
        }
    }
}