using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Klient : AuditableEntity, IHasDomainEvent
    {
        public uint IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string? Email { get; set; }
        public string Salt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }

        public DateTime DataZalozeniaKonta { get; }
        public Account Account { get; }
        public NumerTelefonu NumerTelefonu { get; set; }

        public Klient(uint idOsoba, string imie, string nazwisko, DateTime dataUrodzenia, string numerTelefonu, string? email, string salt, string? refreshToken, 
            DateTime? refreshTokenExp, DateTime dataZalozeniaKonta, string userName, string haslo)
        {
            Account = new Account(userName, haslo);
            DataZalozeniaKonta = DateTimeNow.Now;
            NumerTelefonu = new NumerTelefonu(numerTelefonu);
            IdOsoba = idOsoba;
            Imie = imie;
            Nazwisko = nazwisko;
            DataUrodzenia = dataUrodzenia;
            Email = email;
            Salt = salt;
            RefreshToken = refreshToken;
            RefreshTokenExp = refreshTokenExp;
            DataZalozeniaKonta = dataZalozeniaKonta;
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
