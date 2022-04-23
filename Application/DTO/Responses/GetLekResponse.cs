using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetLekResponse
    {
        public uint IdStanLeku { get; set; }
        public string Nazwa { get; set; }
        public uint Ilosc { get; set; }
        public string JednostkaMiary { get; set; }
        public DateTime DataWaznosci { get; set; }
    }
}
