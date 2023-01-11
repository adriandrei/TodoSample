#region

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoSample.Helpers;
using TodoSample.Models;
using TodoSample.Requests;
using TodoSample.ViewModels;

#endregion

namespace TodoSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator mediator;

        public TodoController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost]
        [Route("create")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create(CreateTodoRequest request)
        {
            await mediator.Send(request);

            return Accepted();
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(Todo), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await mediator.Send(new GetTodoRequest(id));

            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        [Route("list/{key}")]
        [ProducesResponseType(typeof(IEnumerable<TodoListDto>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> List(string? key)
        {
            var result = await mediator.Send(new ListTodoRequest(key));

            return Ok(result.Select(t => new TodoListDto(t)));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            await mediator.Send(new DeleteTodoRequest(id));

            return Accepted();
        }

        [HttpPatch]
        [Route("update/{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTodoDboRequest request)
        {
            await mediator.Send(new UpdateTodoRequest(id, request.Title)
                {Description = request.Description, DueBy = request.DueBy});

            return Accepted();
        }

        [HttpPost]
        [Route("complete/{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Complete(string id)
        {
            await mediator.Send(new CompleteTodoRequest(id));
            return Accepted();
        }
    }
}