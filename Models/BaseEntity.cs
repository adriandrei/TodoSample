namespace TodoSample.Models;

public abstract class BaseEntity
{
    protected BaseEntity(string? id = null)
    {
        id ??= Guid.NewGuid().ToString();

        Id = id;
        Created = Updated = DateTime.UtcNow;
    }

    public virtual string Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}