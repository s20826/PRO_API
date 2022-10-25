﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Szczepienie
    {
        public int IdLek { get; set; }
        public int IdPacjent { get; set; }
        public DateTime DataWaznosci { get; set; }

        public virtual Szczepionka IdLekNavigation { get; set; }
        public virtual Pacjent IdPacjentNavigation { get; set; }
    }
}
