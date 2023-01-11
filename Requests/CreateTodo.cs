#region

using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Models;

#endregion

namespace TodoSample.Requests;

public record CreateTodoRequest : IRequest
{
    public CreateTodoRequest(string title, string? description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
}

public sealed class CreateTodoValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoValidator()
    {
        RuleFor(t => t.Title).NotNull().NotEmpty();
    }
}

public class CreateTodoHandler : IRequestHandler<CreateTodoRequest>
{
    private readonly IRepository<Todo> repository;

    public CreateTodoHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<Unit> Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        repository.Create(new Todo(request.Title, request.Description));

        return Unit.Task;
    }
}