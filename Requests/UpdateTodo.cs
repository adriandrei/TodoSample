using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Models;

namespace TodoSample.Requests;

public class UpdateTodoRequest : IRequest
{
    public UpdateTodoRequest(string id)
    {
        this.Id = id;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateTodoValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoValidator()
    {
        RuleFor(t => t.Id).NotEmpty().NotNull();
        RuleFor(t => t.Title).NotEmpty().NotNull();
    }
}

public class UpdateValidatorHandler : IRequestHandler<UpdateTodoRequest>
{
    private readonly IRepository<Todo> repository;

    public UpdateValidatorHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<Unit> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = this.repository.GetById(request.Id);

        entity.UpdateTitle(request.Title);
        entity.UpdateDescription(request.Description);
        this.repository.Update(entity);

        return Unit.Task;
    }
}
