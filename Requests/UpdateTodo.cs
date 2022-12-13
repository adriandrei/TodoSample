using FluentValidation;
using MediatR;
using TodoSample.Data;
using TodoSample.Helpers;
using TodoSample.Models;

namespace TodoSample.Requests;

public class UpdateTodoRequest : ExistingTodoRequest
{
    public UpdateTodoRequest(string id, string title) : base(id)
    {
        this.Title = title;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueBy { get; set; }
}

public class UpdateTodoValidator : ExistingTodoValidator<UpdateTodoRequest>
{
    public UpdateTodoValidator(IRepository<Todo> repo) : base(repo)
    {
        RuleFor(t => t.Title).NotEmpty().NotNull();
    }
}

public class UpdateValidatorHandler : IRequestHandler<UpdateTodoRequest, Todo>
{
    private readonly IRepository<Todo> repository;

    public UpdateValidatorHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<Todo> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = this.repository.GetById(request.Id);

        entity.UpdateDueBy(request.DueBy);
        entity.UpdateTitle(request.Title);
        entity.UpdateDescription(request.Description);
        this.repository.Update(entity);

        return Task.FromResult(entity);
    }
}
