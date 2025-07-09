namespace Infrastructure;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset InsertedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}