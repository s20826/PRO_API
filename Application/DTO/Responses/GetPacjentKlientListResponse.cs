﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetPacjentKlientListResponse
    {
        public int IdPacjent { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
    }
}