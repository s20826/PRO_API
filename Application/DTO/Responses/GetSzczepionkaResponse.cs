using System;

namespace Application.DTO.Responses
{
    public class GetSzczepionkaResponse
    {
        public string ID_lek { get; set; }
        public string Nazwa { get; set; }
        public string Zastosowanie { get; set; }
        public bool CzyObowiazkowa { get; set; }
        public DateTime? OkresWaznosci { get; set; }
    }
}