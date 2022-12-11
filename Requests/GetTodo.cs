using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Models;

namespace TodoSample.Requests;

public class GetTodoRequest : IRequest<Todo>
{
    public GetTodoRequest(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public sealed class GetTodoValidator : AbstractValidator<GetTodoRequest>
{
    public GetTodoValidator()
    {
        RuleFor(t => t.Id).NotEmpty().NotNull();
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
