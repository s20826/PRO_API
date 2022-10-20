using Domain.Models;
using System;
using System.Collections.Generic;

namespace Test
{
    public class MockData
    {
        public static List<Osoba> GetOsobaList()
        {
            return new List<Osoba>
            {
                new Osoba       //klient
                {
                    IdOsoba = 1,
                    NazwaUzytkownika = "Adam1",
                    Email = "Adam1@gmail.com",
                    NumerTelefonu = "123456789",
                    Salt = "ayH570KmANkGyHqQroN6Nl30mclzaC6Rxfq4SedA+C4="
                },
                new Osoba       //weterynarz
                {
                    IdOsoba = 2,
                    NazwaUzytkownika = "Osoba2",
                    Email = "osoba2@gmail.com",
                    NumerTelefonu = "123456789",
                },
                new Osoba       //admin
                {
                    IdOsoba = 3,
                    NazwaUzytkownika = "Osoba2",
                    Email = "osoba2@gmail.com",
                    NumerTelefonu = "123456789",
                }
            };
        }


        public static List<Klient> GetKlientList()
        {
            return new List<Klient>
            {
                new Klient
                {
                    IdOsoba = 1,
                    DataZalozeniaKonta = DateTime.Now
                }
            };
        }


        public static List<Weterynarz> GetWeterynarzList()
        {
            return new List<Weterynarz>
            {
                new Weterynarz
                {
                    IdOsoba = 2,
                    DataZatrudnienia = DateTime.Now.AddDays(-5),
                    DataUrodzenia = DateTime.Now.AddDays(-10),
                    Pensja = 5000
                }
            };
        }


        public static List<Specjalizacja> GetSpecjalizacjaList()
        {
            return new List<Specjalizacja>
            {
                new Specjalizacja
                {
                    IdSpecjalizacja = 1,
                    Opis = "aaaaaaaaaaaa",
                    Nazwa = "bbbbbbbbbbbb"
                }
            };
        }


        public static List<WeterynarzSpecjalizacja> GetWeterynarzSpecjalizacjaList()
        {
            return new List<WeterynarzSpecjalizacja>
            {
                new WeterynarzSpecjalizacja
                {
                    IdSpecjalizacja = 1,
                    IdOsoba = 2
                }
            };
        }
    }
}