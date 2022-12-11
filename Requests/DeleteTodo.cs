using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Models;

namespace TodoSample.Requests;

public class DeleteTodoRequest : IRequest
{
	public DeleteTodoRequest(string id)
	{
		this.Id = id;
	}

    public string Id { get; set; }
}

public sealed class DeleteTodoValidator : AbstractValidator<DeleteTodoRequest>
{
    public DeleteTodoValidator()
    {
        RuleFor(t => t.Id).NotEmpty().NotNull();
    }
}

public class DeleteTodoHandler : IRequestHandler<DeleteTodoRequest>
{
    private readonly IRepository<Todo> repository;

    public DeleteTodoHandler(IRepository<Todo> repository)
	{
        this.repository = repository;
    }

    public Task<Unit> Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = repository.GetById(request.Id);

        if (entity != null)
        {
            repository.Delete(entity);
        }

        return Unit.Task;
    }
}
