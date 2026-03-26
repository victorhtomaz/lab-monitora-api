using MonitoraAgro.API.Application;
using MonitoraAgro.API.Endpoints;
using MonitoraAgro.API.Infrastructure;
using MonitoraAgro.API.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ResponseExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello");

app.MapUserEndpoints();

app.Run();
