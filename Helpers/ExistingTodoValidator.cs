using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Models;

namespace TodoSample.Helpers;

public abstract class ExistingTodoRequest : IRequest<Todo>
{
    public ExistingTodoRequest(string id)
    {
        this.Id = id;
    }

    public string Id { get; set; }
}

public class ExistingTodoValidator<T> : AbstractValidator<T> where T : ExistingTodoRequest
{
    protected readonly IRepository<Todo> repo;

    public ExistingTodoValidator(IRepository<Todo> repo)
	{
        this.repo = repo;

        RuleFor(t => t.Id).NotEmpty().NotNull();
        RuleFor(t => t.Id).Must(repo.Exists).WithMessage(t => $"Todo {t.Id} doesn't exist");
    }
}
