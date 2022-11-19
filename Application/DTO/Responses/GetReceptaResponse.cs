using System;

namespace Application.DTO.Responses
{
    public class GetReceptaResponse
    {
        public string ID_Recepta { get; set; }
        public string Zalecenia { get; set; }
        public DateTime? WizytaData { get; set; }
    }
}