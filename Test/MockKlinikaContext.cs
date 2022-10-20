using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class MockKlinikaContext
    {
        public static Mock<IKlinikaContext> GetMockDbContext()
        {
            var myDbMoq = new Mock<IKlinikaContext>();
            myDbMoq.Setup(p => p.Osobas).Returns(GetQueryableMockDbSet(MockData.GetOsobaList()));
            myDbMoq.Setup(p => p.Klients).Returns(GetQueryableMockDbSet(MockData.GetKlientList()));
            myDbMoq.Setup(p => p.Weterynarzs).Returns(GetQueryableMockDbSet(MockData.GetWeterynarzList()));
            myDbMoq.Setup(p => p.WeterynarzSpecjalizacjas).Returns(GetQueryableMockDbSet(MockData.GetWeterynarzSpecjalizacjaList()));
            myDbMoq.Setup(p => p.Specjalizacjas).Returns(GetQueryableMockDbSet(MockData.GetSpecjalizacjaList()));
            return myDbMoq;
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
    }
}