using Application.DTO.Responses;
using Application.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PacjentService : IPacjentRepository
    {
        private readonly KlinikaContext context;
        private readonly IConfiguration configuration;
        public PacjentService(KlinikaContext klinikaContext, IConfiguration config)
        {
            context = klinikaContext;
            configuration = config;
        }

        public async Task<List<GetPacjentListResponse>> GetPacjentList()
        {
            return (from x in context.Pacjents
                    join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                    orderby x.Nazwa
                    select new GetPacjentListResponse()
                    {
                        IdOsoba = x.IdOsoba,
                        IdPacjent = x.IdPacjent,
                        Nazwa = x.Nazwa,
                        Gatunek = x.Gatunek,
                        Rasa = x.Rasa,
                        Masc = x.Masc,
                        Wlasciciel = y.Imie + ' ' + y.Nazwisko
                    }).ToList();
        }

        public async Task<List<GetPacjentKlientListResponse>> GetPacjentList(int ID_osoba)
        {
            if (context.Klients.Where(x => x.IdOsoba == ID_osoba).Any() != true)
            {
                throw new Exception("Nie ma klienta o ID = " + ID_osoba);
            }
            
                var results =
                (from x in context.Pacjents
                where x.IdOsoba == ID_osoba
                orderby x.Nazwa
                select new GetPacjentKlientListResponse()
                {
                    IdPacjent = x.IdPacjent,
                    Nazwa = x.Nazwa,
                    Gatunek = x.Gatunek,
                    Rasa = x.Rasa,
                    Masc = x.Masc
                }).ToList();

                return results;
        }

        public async Task<GetPacjentDetails> GetPacjentById(int ID_pacjent)
        {
            if (context.Pacjents.Where(x => x.IdPacjent == ID_pacjent).Any() != true)
            {
                throw new Exception("Nie ma pacjenta o ID = " + ID_pacjent);
            }

            var results =
                (from x in context.Pacjents
                 join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                 where x.IdPacjent == ID_pacjent
                 orderby x.Nazwa
                 select new GetPacjentDetails()
                 {
                     Nazwa = x.Nazwa,
                     Gatunek = x.Gatunek,
                     Rasa = x.Rasa,
                     Masc = x.Masc,
                     Plec = x.Plec,
                     DataUrodzenia = x.DataUrodzenia,
                     Waga = x.Waga,
                     Agresywne = x.Agresywne,
                     Wlasciciel = y.Imie + " " + y.Nazwisko,
                     Wizyty = new PacjentWizytaResponse[0]
                     
                 }).First();

            return results;
        }
    }
}
