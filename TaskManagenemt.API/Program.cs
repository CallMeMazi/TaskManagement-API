using TaskManagement.WebConfig.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapperConfig();

builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("ApplicationConnectionString")!);
builder.Services.AddLogDbContext(builder.Configuration.GetConnectionString("ApplicationLogConnectionString")!);

builder.Services.AddRepositoriesConfig();
builder.Services.AddUnitOfWorkConfig();
builder.Services.AddServicesConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
