using Scalar.AspNetCore;
using TaskManagement.Application.Registration;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Domain.Utilities.Exceptions;
using TaskManagement.Infrastructure.Registration;
using TaskManagement.WebConfig.Register;

var builder = WebApplication.CreateBuilder(args);

// Register System Configs And Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register All Configs And Services In Layers
builder.Services.RegisterAllApplicationLayerConfiguration()
    .RegisterAllInfrastructureLayerConfiguration(builder.Configuration);

// Exception Handler Configs And ProblemDetails Service
builder.Services.RegisterExceptionHandlersAndServices();

var app = builder.Build();

// Compile AutoMapper Configs After Starting Application
app.Services.CompileAutoMapperConfiguration();

// System Midlewares
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    SuppressDiagnosticsCallback = context => context.Exception is AppException or DomainException
});
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
