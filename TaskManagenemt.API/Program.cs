using Scalar.AspNetCore;
using Serilog;
using TaskManagement.Application.Registration;
using TaskManagement.Infrastructure.Registration;
using TaskManagement.WebConfig.Registeration;

var builder = WebApplication.CreateBuilder(args);

// Register System Configs And Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register All Configs And Services In Layers
builder.Services.RegisterAllApplicationLayerConfiguration()
    .RegisterAllInfrastructureLayerConfiguration(builder.Configuration);

// Register Serilog Congigs And Services
builder.Host.RegisterSerilog(builder.Services);

// Exception Handler Configs And ProblemDetails Service
builder.Services.RegisterExceptionHandlersAndServices();

var app = builder.Build();

// Compile AutoMapper Configs After Starting Application
app.Services.CompileAutoMapperConfiguration();

app.UseApplicationExceptionHandler();

// System Midlewares
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
