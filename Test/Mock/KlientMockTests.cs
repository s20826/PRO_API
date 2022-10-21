using Application.Interfaces;
using Application.Klienci.Commands;
using HashidsNet;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Mock
{
    public class KlientMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;
        public IConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
            var inMemorySettings = new Dictionary<string, string> {
                {"SecretKey", "q4Ze7tyWVopasdfghjkPnr6uvpapajwEz3m18nqu6cA41qaz2wsx3edc4rfvplijygrdwa2137xd2OChybfthvFcdf"},
                {"PasswordIterations", "150000"}
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        /*[Test]
        public async Task CreateKlientShouldBeCorrectTest()
        {
            var handler = new CreateKlientCommandHandler(mockContext.Object, new PasswordService(), configuration);

            CreateKlientCommand command = new CreateKlientCommand()
            {
                request = new Application.DTO.KlientCreateRequest
                {
                    Imie = "aaa",
                    Nazwisko = "bbb",
                    NumerTelefonu = "777888999",
                    Email = "aaa.bbb@gmail.com",
                    NazwaUzytkownika = "NewUsername",
                    Haslo = "Password1",
                    Haslo2 = "Password1"
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(2, mockContext.Object.Klients.Count());
        }*/


        [Test]
        public async Task DeleteKlientShouldBeCorrectTest()
        {
            var handler = new DeleteKlientCommandHandle(mockContext.Object, hash);

            DeleteKlientCommand command = new DeleteKlientCommand()
            {
                ID_osoba = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(1, mockContext.Object.Klients.Count());
        }


        [Test]
        public void DeleteKlientShouldThrowAnExceptionTest()
        {
            var handler = new DeleteKlientCommandHandle(mockContext.Object, hash);

            DeleteKlientCommand command = new DeleteKlientCommand()
            {
                ID_osoba = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
            Assert.AreEqual(1, mockContext.Object.Klients.Count());
        }
    }
}