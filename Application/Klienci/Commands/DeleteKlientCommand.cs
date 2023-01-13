using Application.Common.Exceptions;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.IO;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Klienci.Commands
{
    public class DeleteKlientCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteKlientCommandHandle : IRequestHandler<DeleteKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetKlientListResponse> cache;
        public DeleteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<int> Handle(DeleteKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var osoba = context.Osobas.FirstOrDefault(x => x.IdOsoba == id);
            var klient = context.Klients.FirstOrDefault(x => x.IdOsoba == id);

            if (!osoba.Rola.Equals("K"))
            {
                throw new Exception("");
            }

            if(context.Wizyta.Where(x => x.IdOsoba== id && !x.CzyOplacona && x.Status.Equals(WizytaStatus.Zrealizowana.ToString())).Any()) 
            {
                throw new Exception("Klient ma nieopłaconą wizytę");
            }
            
            osoba.Nazwisko = osoba.Nazwisko.ElementAt(0).ToString();
            osoba.Haslo = "";
            osoba.Salt = "";
            osoba.RefreshToken = new Guid().ToString();
            osoba.Email = "";
            osoba.NumerTelefonu = "";

            using (StreamWriter fileStream = new StreamWriter(new FileStream(@"Klienci.log", FileMode.Append)))
            {
                
                string outputString = osoba.Imie + " " + osoba.Nazwisko.ElementAt(0).ToString() + 
                    " (" + klient.DataZalozeniaKonta.ToShortDateString() + " - " + DateTime.Now.ToShortDateString() + ")" + "\n";

                string total = "koszt wizyt: " + context.Wizyta.Where(x => x.IdOsoba == id).Sum(x => x.Cena).ToString() + "\n";
                string stats = "liczba wizyt: " + context.Wizyta.Where(x => x.IdOsoba == id).Count() + "\n" + "\n";

                await fileStream.WriteAsync(outputString + total + stats);
            }

            return 0;

            //cache.Remove();
            //return await context.SaveChangesAsync(cancellationToken);
        }
    }
}