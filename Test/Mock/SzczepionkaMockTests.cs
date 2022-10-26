﻿using Application.DTO.Requests;
using Application.Interfaces;
using Application.Szczepionki.Commands;
using HashidsNet;
using Infrastructure.Services;
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
    public class SzczepionkaMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
        }


        /*[Test]
        public async Task CreateChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Chorobas.Count();
            var handler = new CreateSzczepionkaCommandHandler(mockContext.Object, hash);

            var command = new CreateSzczepionkaCommand()
            {
                request = new SzczepionkaRequest
                {
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Chorobas.Count(), before + 1);
        }*/


        [Test]
        public async Task UpdateSzczepionkaShouldBeCorrectTest()
        {
            var handler = new UpdateSzczepionkaCommandHandler(mockContext.Object, hash);

            var command = new UpdateSzczepionkaCommand()
            {
                ID_szczepionka = hash.Encode(2),
                request = new SzczepionkaRequest
                {
                    Nazwa = "aaa",
                    CzyObowiazkowa = true,
                    Zastosowanie = "...",
                    Producent = "...",
                    OkresWaznosci = new DateTime(1,1,1)
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateSzczepionkaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateSzczepionkaCommandHandler(mockContext.Object, hash);

            var command = new UpdateSzczepionkaCommand()
            {
                ID_szczepionka = hash.Encode(-1),
                request = new SzczepionkaRequest
                {
                    Nazwa = "aaa",
                    CzyObowiazkowa = true,
                    Zastosowanie = "..."
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteSzczepionkaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Szczepionkas.Count();
            var handler = new DeleteSzczepionkaCommandHandler(mockContext.Object, hash);

            var command = new DeleteSzczepionkaCommand()
            {
                ID_szczepionka = hash.Encode(2)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Szczepionkas.Count());
        }


        [Test]
        public void DeleteSzczepionkaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteSzczepionkaCommandHandler(mockContext.Object, hash);

            var command = new DeleteSzczepionkaCommand()
            {
                ID_szczepionka = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}