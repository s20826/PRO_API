﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetHarmonogramWeterynarzResponse
    {
        public string IdHarmonogram { get; set; }
        public string? IdKlient { get; set; }
        public string? Klient { get; set; }
        public string? IdPacjent { get; set; }
        public string? Pacjent { get; set; }
        public bool CzyZajete { get; set; }
        public DateTime Data { get; set; }
    }
}
