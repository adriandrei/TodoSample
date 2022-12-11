namespace TodoSample.Models;

public class Todo : BaseEntity
{
    public Todo(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; set; }
    public string? Description { get; set; }

    public void UpdateTitle(string title)
    {
        this.Title = title;
    }
    
    public void UpdateDescription(string? description)
    {
        this.Description = description;
    }
}
