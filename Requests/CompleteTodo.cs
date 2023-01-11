#region

using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Helpers;
using TodoSample.Models;

#endregion

namespace TodoSample.Requests;

public class CompleteTodoRequest : ExistingTodoRequest
{
    public CompleteTodoRequest(string id) : base(id)
    {
    }
}

public sealed class CompleteTodoRequestValidator : ExistingTodoValidator<CompleteTodoRequest>
{
    public CompleteTodoRequestValidator(IRepository<Todo> repo) : base(repo)
    {
        RuleFor(t => t.Id).Must(RequestIsNotCompleted).WithMessage(t => $"Todo {t.Id} already completed");
    }

    private bool RequestIsNotCompleted(string id)
    {
        var todo = repo.GetById(id);

        return !todo.Completed;
    }
}

public class CompleteTodoRequestHandler : IRequestHandler<CompleteTodoRequest, Todo>
{
    private readonly IRepository<Todo> repo;

    public CompleteTodoRequestHandler(IRepository<Todo> repo)
    {
        this.repo = repo;
    }

    public Task<Todo> Handle(CompleteTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = repo.GetById(request.Id);
        todo.Complete(true);
        repo.Update(todo);

        return Task.FromResult(todo);
    }
}