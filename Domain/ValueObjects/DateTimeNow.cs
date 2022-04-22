using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record DateTimeNow
    {
        public static DateTime Now => DateTime.Now;
    }
}
