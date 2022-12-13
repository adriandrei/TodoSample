using TodoSample.Models;

namespace TodoSample.ViewModels;

public record UpdateTodoDboRequest
{
    public UpdateTodoDboRequest(string title)
    {
        this.Title = title;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueBy { get; set; }
}

public record TodoListDto
{
    public TodoListDto(Todo todo)
    {
        this.Id = todo.Id;
        this.Title = todo.Title;
        this.DueBy = todo.DueBy;
        this.Updated = todo.Updated;
        this.Completed = todo.Completed;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime Updated { get; set; }
    public DateTime? DueBy { get; set; }
    public bool Completed { get; set; }
}
