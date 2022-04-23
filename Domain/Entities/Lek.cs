using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lek : AuditableEntity, IHasDomainEvent
    {
        public uint IdLek { get; }
        public LekNazwa Nazwa { get; set; }
        public LekJednostka JednostkaMiary { get; set; }
        private uint _Ilosc { get; set; }
        public DateTime DataWaznosci { get; set; }

        public uint Ilosc
        {
            get => _Ilosc;
            set
            {
                if(value < 10)
                {
                    DomainEvents.Add(new TooFewLekEvent(this));
                }

                _Ilosc = value;
            }
        }

        
        public Lek(uint idLek, LekNazwa nazwa, LekJednostka jednostkaMiary, uint ilosc, DateTime dataWaznosci)
        {
            if(dataWaznosci < DateTimeNow.Now)
            {
                throw new LekDataWaznosciException("Wprowadzona data ważności jest ...");
            }
            IdLek = idLek;
            Nazwa = nazwa;
            JednostkaMiary = jednostkaMiary;
            Ilosc = ilosc;
            DataWaznosci = dataWaznosci;
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
