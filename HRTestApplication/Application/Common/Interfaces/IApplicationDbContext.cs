using HRTestApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRTestApplication.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Division> Divisions { get; }

        DbSet<Candidate> Candidates { get; }

        DbSet<Vacancy> Vacancies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
