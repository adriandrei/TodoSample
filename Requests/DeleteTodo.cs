#region

using MediatR;
using TodoSample.Data;
using TodoSample.Helpers;
using TodoSample.Models;

#endregion

namespace TodoSample.Requests;

public class DeleteTodoRequest : ExistingTodoRequest
{
    public DeleteTodoRequest(string id) : base(id)
    {
    }
}

public sealed class DeleteTodoValidator : ExistingTodoValidator<DeleteTodoRequest>
{
    public DeleteTodoValidator(IRepository<Todo> repo) : base(repo)
    {
    }
}

public class DeleteTodoHandler : IRequestHandler<DeleteTodoRequest, Todo>
{
    private readonly IRepository<Todo> repository;

    public DeleteTodoHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<Todo> Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = repository.GetById(request.Id);
        repository.Delete(entity);
        return Task.FromResult(entity);
    }
}