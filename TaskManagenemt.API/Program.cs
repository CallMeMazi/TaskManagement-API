using Scalar.AspNetCore;
using Serilog;
using TaskManagement.Application.Registration;
using TaskManagement.Common.Settings;
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

builder.Services.RegisterJwtBearer(builder.Configuration.GetValue<AppSettings>("AppSettings")!);
builder.Services.RegisterExceptionHandlersAndServices();
builder.Host.RegisterSerilog(builder.Services);

var app = builder.Build();

app.Services.CompileAutoMapperConfiguration();
app.UseApplicationExceptionHandler();

// System Midlewares
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthentication();
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
