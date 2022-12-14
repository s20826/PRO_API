using Application.Interfaces;
using Application.WizytaUslugi.Commands;
using HashidsNet;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Mock
{
    public class WizytaUslugaMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
        }

        [Test]
        public async Task AddWizytaUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaUslugas.Count();
            var handler = new AddWizytaUslugaCommandHandler(mockContext.Object, hash, new WizytaService());

            AddWizytaUslugaCommand command = new AddWizytaUslugaCommand()
            {
                ID_usluga = hash.Encode(2),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            Assert.AreEqual(before + 1, mockContext.Object.WizytaUslugas.Count());
        }

        [Test]
        public async Task RemoveWizytaUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaUslugas.Count();
            var handler = new RemoveWizytaUslugaCommandHandler(mockContext.Object, hash, new WizytaService());

            RemoveWizytaUslugaCommand command = new RemoveWizytaUslugaCommand()
            {
                ID_usluga = hash.Encode(1),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            Assert.AreEqual(before - 1, mockContext.Object.WizytaUslugas.Count());
        }

        [Test]
        public void RemoveWizytaUslugaShouldThrowAnExceptionTest()
        {
            var handler = new RemoveWizytaUslugaCommandHandler(mockContext.Object, hash, new WizytaService());

            RemoveWizytaUslugaCommand command = new RemoveWizytaUslugaCommand()
            {
                ID_usluga = hash.Encode(-1),
                ID_wizyta = hash.Encode(1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}