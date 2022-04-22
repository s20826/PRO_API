using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Weterynarz : AuditableEntity, IHasDomainEvent
    {
        public uint IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string? Email { get; set; }
        public string Salt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
        public decimal Pensja { get; set; }
        public DateTime DataZatrudnienia { get; set; }

        public Account Account { get; }
        public NumerTelefonu NumerTelefonu { get; set; }

        public Weterynarz(uint idOsoba, string imie, string nazwisko, DateTime dataUrodzenia, string numerTelefonu, string? email, string salt, string? refreshToken,
            DateTime? refreshTokenExp, decimal pensja, DateTime dataZatrudnienia, string userName, string haslo)
        {
            Account = new Account(userName, haslo);
            NumerTelefonu = new NumerTelefonu(numerTelefonu);
            IdOsoba = idOsoba;
            Imie = imie;
            Nazwisko = nazwisko;
            DataUrodzenia = dataUrodzenia;
            Email = email;
            Salt = salt;
            RefreshToken = refreshToken;
            RefreshTokenExp = refreshTokenExp;
            Pensja = pensja;
            DataZatrudnienia = dataZatrudnienia;
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
