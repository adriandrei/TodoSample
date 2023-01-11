#region

using MediatR;
using TodoSample.Data;
using TodoSample.Models;

#endregion

namespace TodoSample.Requests;

public class ListTodoRequest : IRequest<IEnumerable<Todo>>
{
    public ListTodoRequest(string? key)
    {
        Key = key;
    }

    public string? Key { get; set; }
}

public class ListTodoHandler : IRequestHandler<ListTodoRequest, IEnumerable<Todo>>
{
    private readonly IRepository<Todo> repository;

    public ListTodoHandler(IRepository<Todo> repository)
    {
        this.repository = repository;
    }

    public Task<IEnumerable<Todo>> Handle(ListTodoRequest request, CancellationToken cancellationToken)
    {
        var result = request.Key == null
            ? repository.List()
            : repository.List(t => t.Value!.Title!.Contains(request.Key!));

        return Task.FromResult(result);
    }
}