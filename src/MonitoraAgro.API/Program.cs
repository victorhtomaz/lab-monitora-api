using Microsoft.AspNetCore.Authentication;
using MonitoraAgro.API.Application;
using MonitoraAgro.API.Core.Endpoints;
using MonitoraAgro.API.Core.Handlers;
using MonitoraAgro.API.Core.Middlewares;
using MonitoraAgro.API.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", _ => { });

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello");

app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapMeasurementsEndpoints();

app.Run();
