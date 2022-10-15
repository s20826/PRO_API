using Domain.Enums;
using Domain.Models;
using Infrastructure.Services;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services
{
    public class WizytaServiceTests
    {
        private static object[] GetwizytaLists1()
        {
            IList<Wizytum> wizytaList = new List<Wizytum>(){
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                }
            };

            return new[] { wizytaList };
        }

        private static object[] GetwizytaLists2()
        {
            IList<Wizytum> wizytaList = new List<Wizytum>(){
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = WizytaStatus.Zaplanowana.ToString()
                }
            };

            return new[] { wizytaList };
        }


        [Test]
        [TestCaseSource("GetwizytaLists1")]
        public void WizytaAbleToCreateTest(List<Wizytum> a)
        {
            var result = new WizytaService().IsWizytaAbleToCreate(a);
            Assert.IsTrue(result);
        }


        [Test]
        [TestCaseSource("GetwizytaLists2")]
        public void WizytaNotAbleToCreateTest(List<Wizytum> a)
        {
            var result = new WizytaService().IsWizytaAbleToCreate(a);
            Assert.IsFalse(result);
        }


        [Test]
        [TestCase(-5,true)]
        [TestCase(-3, false)]
        [TestCase(-4, true)]
        public void IsWizytaAbleToCancelTest(int hour, bool expectedResult)
        {
            var result = new WizytaService().IsWizytaAbleToCancel(DateTime.Now.AddHours(hour));
            Assert.AreEqual(expectedResult, result);
        }
    }
}
