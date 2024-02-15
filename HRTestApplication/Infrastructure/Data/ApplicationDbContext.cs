using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HRTestApplication.Domain.Entities;
using HRTestApplication.Application.Common.Interfaces;

namespace HRTestApplication.Infrastructure.Data;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Division> Divisions => Set<Division>();

    public DbSet<Candidate> Candidates => Set<Candidate>();

    public DbSet<Vacancy> Vacancies => Set<Vacancy>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
