using System;
using System.Collections.Generic;

#nullable disable

namespace PRO_API.Models
{
    public partial class ZnizkaRequest
    {
        public string NazwaZnizki { get; set; }
        public float ProcentZnizki { get; set; }
        public DateTime? DoKiedy { get; set; }
    }
}
