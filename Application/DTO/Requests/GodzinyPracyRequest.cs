using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests
{
    public class GodzinyPracyRequest
    {
        public string DzienTygodnia { get; set; }
        public TimeSpan GodzinaRozpoczecia { get; set; }
        public TimeSpan GodzinaZakonczenia { get; set; }
    }
}
