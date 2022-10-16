using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Services;
using NUnit.Framework;

namespace Test.Services
{
    public class LoginServiceTests
    {
        private int iterations = 150000;
        private string haslo = "Adam1";

        private static object[] GetOsoba1()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyK9VxDN5/+faDZwWUuT2xLbDv0gegrWAg==",
                Salt = "ayH570KmANkGyHqQroN6Nl30mclzaC6Rxfq4SedA+C4="
            };

            return new [] { osoba1 };
        }

        private static object[] GetOsoba2()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyfdgK9VxDNZwWUuT2xLbDv0gegr111",
                Salt = "j666AjTc1HgWsLDptN4w+V9oSP+zWFYpkAVCgFsXiM0="
            };

            return new[] { osoba1, null };
        }

        [Test]
        [TestCaseSource("GetOsoba1")]
        public void LoginDoesNotThrowAnException(object o)
        {
            Assert.DoesNotThrow(() => new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations));
        }

        [Test]
        [TestCaseSource("GetOsoba2")]
        public void LoginThrowsAnException(object o)
        {
            Assert.Throws<UserNotAuthorizedException>(() => new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations));
        }
    }
}