using TaskManagement.WebConfig.DI;

var builder = WebApplication.CreateBuilder(args);

// System Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Framework Services
builder.Services.AddAutoMapperConfig();

// DbContext Services
builder.Services.AddDbContexts(builder.Configuration);

// Custom Services
builder.Services.AddAppServicesConfig(builder.Configuration);

var app = builder.Build();

app.Services.CompileMappings();

//Custom Midleware

// System Midleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
