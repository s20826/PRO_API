using System;

namespace Application.DTO.Requests
{
    public class SzczepionkaRequest
    {
        public string? Producent { get; set; }
        public string Zastosowanie { get; set; }
        public bool CzyObowiazkowa { get; set; }
        public TimeSpan? OkresWaznosci { get; set; }
    }
}