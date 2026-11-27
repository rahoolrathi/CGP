using CgbSteps.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Data;

/// <summary>
/// Adds fake departments and employees for local testing. Run with: dotnet run -- seed
/// Only works in Development and only when there are no employees yet.
/// </summary>
public sealed partial class DevDataSeeder(
    AppDbContext db,
    IHostEnvironment environment,
    TimeProvider clock,
    ILogger<DevDataSeeder> logger)
{
    private const int EmployeeCount = 60;

    private static readonly string[] DepartmentNames =
    [
        "Finance", "Human Resources", "Information Technology", "Operations",
        "Marketing", "Legal", "Risk Management", "Customer Service",
    ];

    private static readonly string[] FirstNames =
    [
        "Ahmed", "Fatima", "Mohammed", "Aisha", "Khalid", "Maryam", "Omar", "Noora",
        "Hamad", "Sara", "Ali", "Hessa", "Yousef", "Latifa", "Jassim", "Reem",
        "Ravi", "Priya", "John", "Maria",
    ];

    private static readonly string[] LastNames =
    [
        "Al-Thani", "Al-Kuwari", "Al-Marri", "Al-Sulaiti", "Al-Mohannadi", "Al-Naimi",
        "Al-Hajri", "Al-Emadi", "Nair", "Fernandes", "Smith", "Khan",
    ];

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!environment.IsDevelopment())
        {
            throw new InvalidOperationException("Fake data can only be added in Development.");
        }

        if (await db.Employees.AnyAsync(cancellationToken))
        {
            LogSkipped(logger);
            return;
        }

        var now = clock.GetUtcNow();
        var existing = await db.Departments.ToDictionaryAsync(d => d.Name, cancellationToken);
        var departments = DepartmentNames
            .Select(name => existing.TryGetValue(name, out var department) ? department : Department.Create(name, now))
            .ToList();
        db.Departments.AddRange(departments.Where(d => !existing.ContainsKey(d.Name)));

        for (var i = 0; i < EmployeeCount; i++)
        {
            var first = FirstNames[i % FirstNames.Length];
            var last = LastNames[(i * 7) % LastNames.Length];
            var number = 1001 + i;

            db.Employees.Add(Employee.Create(
                employeeId: $"CGB{number}",
                fullName: $"{first} {last}",
                workEmail: $"employee{number}@example.com",
                departmentId: departments[i % departments.Count].Id,
                now: now));
        }

        await db.SaveChangesAsync(cancellationToken);
        LogSeeded(logger, departments.Count, EmployeeCount);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Employees already exist, so no fake data was added")]
    private static partial void LogSkipped(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Added fake data: {Departments} departments, {Employees} employees")]
    private static partial void LogSeeded(ILogger logger, int departments, int employees);
}
