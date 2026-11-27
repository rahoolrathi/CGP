namespace CgbSteps.Api.Models.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; } = Guid.CreateVersion7();
}
