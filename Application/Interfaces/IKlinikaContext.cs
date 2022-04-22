using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IKlinikaContext
    {
        DbSet<Lek> Leks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
