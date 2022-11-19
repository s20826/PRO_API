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

        public void SendPrzypomnienieEmail(string to)
        {
            emailSender.SendPrzypomnienieEmail(to, DateTime.Now, "aaaa bbbb");
            logger.LogInformation("Email sent to " + to + " at: " + DateTime.Now.ToString());
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