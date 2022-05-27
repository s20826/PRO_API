using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class GlobalValues
    {
        public static int DNI_PRACY = 5;
        public static TimeSpan GODZINA_ROZPOCZECIA_PRACY = new TimeSpan(9, 0, 0);
        public static TimeSpan GODZINA_ZAKONCZENIA_PRACY = new TimeSpan(17, 0, 0);
    }
}
