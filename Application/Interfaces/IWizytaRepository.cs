using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IWizytaRepository
    {
        bool IsWizytaAbleToCreate(List<Wizytum> wizytaList);

        bool IsWizytaAbleToCancel(DateTime wizytaDate);
    }
}
