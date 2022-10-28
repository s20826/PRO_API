using Domain.Models;
using Infrastructure.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services
{
    public class HarmonogramServiceTests
    {
        private static object[] GetGodzinyPracy1()
        {
            GodzinyPracy godzinyPracy =
                new GodzinyPracy
                {
                    GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                    GodzinaZakonczenia = new TimeSpan(12, 0, 0)
                };

            return new[] { godzinyPracy };
        }

        private static object[] GetGodzinyPracy2()
        {
            GodzinyPracy godzinyPracy =
                new GodzinyPracy
                {
                    GodzinaRozpoczecia = new TimeSpan(9, 12, 0),
                    GodzinaZakonczenia = new TimeSpan(12, 0, 0)
                };

            return new[] { godzinyPracy };
        }

        [Test]
        [TestCaseSource("GetGodzinyPracy1")]
        public void HarmonogramShouldBeCorrectTest(GodzinyPracy a)
        {
            var result = new HarmonogramService().HarmonogramCount(a);
            Assert.AreEqual(result, 6);
        }

        [Test]
        [TestCaseSource("GetGodzinyPracy2")]
        public void HarmonogramThrowsAnExceptionTest(GodzinyPracy a)
        {
            Assert.Throws<Exception>(() => new HarmonogramService().HarmonogramCount(a));
        }
    }
}