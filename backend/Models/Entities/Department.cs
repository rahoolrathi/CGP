namespace CgbSteps.Api.Models.Entities;

public sealed class Department
{
    private Department()
    {
    }

    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public string Name { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    public ICollection<Employee> Employees { get; } = new List<Employee>();

    public static Department Create(string name, DateTimeOffset now) => new()
    {
        Name = name,
        CreatedAt = now,
    };
}
