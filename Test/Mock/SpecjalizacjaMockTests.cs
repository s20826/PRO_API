﻿using Application.DTO.Request;
using Application.Interfaces;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
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
    public class SpecjalizacjaMockTests
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
        public async Task GetSpecjalizacjaTest()
        {
            var handler = new SpecjalizacjaListQueryHandle(mockContext.Object, hash);
            var result = await handler.Handle(new SpecjalizacjaListQuery(), CancellationToken.None);

            Assert.AreEqual(result.Count(), 1);
        }


        [Test]
        public async Task CreateSpecjalizacjaShouldBeCorrectTest()
        {
            var handler = new CreateSpecjalizacjaCommandHandle(mockContext.Object, hash);

            var command = new CreateSpecjalizacjaCommand()
            {
                request = new SpecjalizacjaRequest
                {
                    Opis = "aaa",
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Specjalizacjas.Count(), 2);
        }


        [Test]
        public async Task UpdateSpecjalizacjaShouldBeCorrectTest()
        {
            var handler = new UpdateSpecjalizacjaCommandHandle(mockContext.Object, hash);

            var command = new UpdateSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(1),
                request = new SpecjalizacjaRequest
                {
                    Opis = "update",
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateSpecjalizacjaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateSpecjalizacjaCommandHandle(mockContext.Object, hash);

            var command = new UpdateSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(-1),
                request = new SpecjalizacjaRequest
                {
                    Opis = "update",
                    Nazwa = "aaa"
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteSpecjalizacjaShouldBeCorrectTest()
        {
            var handler = new DeleteSpecjalizacjaCommandHandle(mockContext.Object, hash);

            var command = new DeleteSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void DeleteSpecjalizacjaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteSpecjalizacjaCommandHandle(mockContext.Object, hash);

            var command = new DeleteSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}