using RedisNet8Api.Data;
using RedisNet8Api.Services.Shared;
using RedisNet8Api.Services.Student;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IApplicationContextDapper, ApplicationContextDapper>();
builder.Services.AddScoped<ICacheService, CacheService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
