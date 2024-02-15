using HRTestApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRTestApplication.Infrastructure.Data;
public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        if (!_context.Divisions.Any())
        {
            _context.Divisions.AddRange([
                new Division
                {
                    Name = "Отдел №1",
                    Vacancies = [new() { Description = "Описание вакансии 1" }]
                },
                new Division
                {
                    Name = "Отдел №2"
                },
                new Division
                {
                    Name = "Отдел №3"
                },
                new Division
                {
                    Name = "Отдел №4",
                    Vacancies = [
                        new() { Description = "Описание вакансии 2", StepsActive = 14 },
                        new() { Description = "Описание вакансии 3" }
                    ]
                },
                new Division
                {
                    Name = "Отдел №5"
                },
                new Division
                {
                    Name = "Отдел №6"
                },
                new Division
                {
                    Name = "Отдел №7"
                },
                new Division
                {
                    Name = "Отдел №8"
                },
                new Division
                {
                    Name = "Отдел №9"
                },
                new Division
                {
                    Name = "Отдел №10"
                }
            ]);
        }

        await _context.SaveChangesAsync();
    }
}
