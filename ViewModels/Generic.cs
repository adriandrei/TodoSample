#region

using TodoSample.Models;

#endregion

namespace TodoSample.ViewModels;

public record UpdateTodoDboRequest
{
    public UpdateTodoDboRequest(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueBy { get; set; }
}

public record TodoListDto
{
    public TodoListDto(Todo todo)
    {
        Id = todo.Id;
        Title = todo.Title;
        DueBy = todo.DueBy;
        Updated = todo.Updated;
        Completed = todo.Completed;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime Updated { get; set; }
    public DateTime? DueBy { get; set; }
    public bool Completed { get; set; }
}