using Application.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HarmonogramService : IHarmonogramRepository
    {
        public int HarmonogramCount(GodzinyPracy godziny)
        {
            var result = godziny.GodzinaZakonczenia.Subtract(godziny.GodzinaRozpoczecia);

            if(result.TotalMinutes % 30 != 0)
            {
                throw new Exception();
            }


            return (int)(result.TotalMinutes/30);
        }
    }
}