using TaskManagement.Application.Registration;
using TaskManagement.Infrastructure.Registration;

var builder = WebApplication.CreateBuilder(args);

// Register System Configs And Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register All Configs And Services In Layers
builder.Services.RegisterAllApplicationLayerConfiguration()
    .RegisterAllInfrastructureLayerConfiguration(builder.Configuration);

var app = builder.Build();

// Compile AutoMapper Configs After Starting Application
app.Services.CompileAutoMapperConfiguration();

//Custom Midlewares

// System Midlewares
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
