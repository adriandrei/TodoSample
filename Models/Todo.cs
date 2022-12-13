namespace TodoSample.Models;

public class Todo : BaseEntity
{
    public Todo(string title, string? description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueBy { get; set; }
    public bool Completed { get; set; } = false;


    public void UpdateDueBy(DateTime? dueBy)
    {
        DueBy = dueBy;
    }

    public void UpdateTitle(string title)
    {
        this.Title = title;
    }
    
    public void UpdateDescription(string? description)
    {
        this.Description = description;
    }

    public void Complete(bool completed)
    {
        this.Completed = completed;
    }
}
