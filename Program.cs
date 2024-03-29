#region

using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using TodoSample;
using TodoSample.Data;
using TodoSample.Helpers;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Api", Version = "v1"});
});

builder.Services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheck>("health");

var app = builder.Build();

if (!app.Environment.IsDevelopment()) 
    app.UseResponseCompression();

app.UseCors(t => t.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.MapHealthChecks("/health");
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);
app.Run();