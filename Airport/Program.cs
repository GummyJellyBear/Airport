using Airport.BL;
using Airport.Hubs;
using Airport.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAirplaneService,AirplaneService>();
builder.Services.AddTransient<ITimerService, TimerService>();
builder.Services.AddSingleton<ITowerControl, TowerControl>();
builder.Services.AddSingleton<IPathControl, PathControl>();
builder.Services.AddSingleton<IAirControl, AirControl>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myHub");

app.Run();
