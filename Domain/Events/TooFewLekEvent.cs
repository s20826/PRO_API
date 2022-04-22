using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class TooFewLekEvent : DomainEvent
    {
        public Lek Lek { get; set; }

        public TooFewLekEvent(Lek lek)
        {
            Lek = lek;
        }
    }
}
