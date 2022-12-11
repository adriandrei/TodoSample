using FluentValidation;
using System.Text.Json;

namespace TodoSample.Helpers;

internal record class ExceptionResponse
{
    public static ExceptionResponse From(int status) => new ExceptionResponse { Status = status };

    public string Title { get; set; }
    public int Status { get; set; }
    public IEnumerable<object> Errors { get; set; }
    public Guid TraceId { get; set; }
}
internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var traceId = Guid.NewGuid();
        try
        {

            context.Response.Headers.Add("x-traceId", traceId.ToString());
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"traceId: {traceId}\n{e.Message}");
            await HandleExceptionAsync(context, e, traceId);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, Guid traceId)
    {
        var statusCode = GetStatusCode(exception);
        var response = new ExceptionResponse
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Errors = GetErrors(exception),
            TraceId = traceId
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ValidationException => "Bad Request",
            ApplicationException applicationException => applicationException.Message,
            _ => "Server Error"
        };
    private static IEnumerable<object> GetErrors(Exception exception)
    {
        IEnumerable<object> errors = null;
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.Select(t => new { PropertyName = t.PropertyName, ErrorMessage = t.ErrorMessage });
        }
        else
        {
            errors = new List<object> { "Internal Server Error" };
        }
        return errors;
    }
}
