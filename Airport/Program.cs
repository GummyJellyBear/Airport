using Airport.BL;
using Airport.BL.BLIntefeces;
using Airport.BL.StationControl;
using Airport.Hubs;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "airportcors", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
builder.Services.AddSignalR();
builder.Services.AddTransient<ITimerService, TimerService>();
builder.Services.AddSingleton<IAirplaneService,AirplaneService>();
builder.Services.AddSingleton<IAirControl, AirControl>();
builder.Services.AddSingleton<IPathControl, PathControl>();
builder.Services.AddTransient<IStation, Station>();
builder.Services.AddSingleton<ITowerControl, TowerControl>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("airportcors");
app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myHub");

app.Run();