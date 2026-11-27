namespace CgbSteps.Api.Models.Entities;

public sealed class Employee
{
    private Employee()
    {
    }

    public Guid Id { get; private set; } = Guid.CreateVersion7();

    /// <summary>The company employee ID, stored in upper case (for example CGB1234).</summary>
    public string EmployeeId { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    /// <summary>Work email, stored in lower case.</summary>
    public string WorkEmail { get; private set; } = string.Empty;

    public Guid DepartmentId { get; private set; }

    public Department Department { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; }

    public static Employee Create(
        string employeeId,
        string fullName,
        string workEmail,
        Guid departmentId,
        DateTimeOffset now) => new()
    {
        EmployeeId = employeeId,
        FullName = fullName,
        WorkEmail = workEmail,
        DepartmentId = departmentId,
        CreatedAt = now,
    };

    public void ChangeDepartment(Guid departmentId) => DepartmentId = departmentId;
}
