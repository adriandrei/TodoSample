#region

using MediatR;
using TodoSample.Data;
using TodoSample.Helpers;
using TodoSample.Models;

#endregion

namespace TodoSample.Requests;

public class GetTodoRequest : ExistingTodoRequest
{
    public GetTodoRequest(string id) : base(id)
    {
    }
}

public sealed class GetTodoValidator : ExistingTodoValidator<GetTodoRequest>
{
    public GetTodoValidator(IRepository<Todo> repo) : base(repo)
    {
    }
}

public class GetTodoHandler : IRequestHandler<GetTodoRequest, Todo>
{
    private readonly IRepository<Todo> repository;

    public GetTodoHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<Todo> Handle(GetTodoRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.GetById(request.Id));
    }
}