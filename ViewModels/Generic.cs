namespace TodoSample.ViewModels;

public record UpdateTodoDboRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public record TodoListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime Updated { get; set; }
}
