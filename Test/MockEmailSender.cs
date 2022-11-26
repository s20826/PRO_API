using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MockEmailSender : IEmailSender
    {
        public Task SendAnulujWizyteEmail(string to, DateTime data)
        {
            return Task.CompletedTask;
        }

        public Task SendCreateAccountEmail(string to)
        {
            throw new NotImplementedException();
        }

        public Task SendHasloEmail(string to, string content)
        {
            throw new NotImplementedException();
        }

        public Task SendPrzypomnienieEmail(string to, DateTime data, string weterynarz)
        {
            throw new NotImplementedException();
        }

        public Task SendSzczepienieEmail(string to, DateTime data, string pacjent)
        {
            throw new NotImplementedException();
        }

        public Task SendUmowWizytaEmail(string to, DateTime data, string weterynarz)
        {
            throw new NotImplementedException();
        }
    }
}
