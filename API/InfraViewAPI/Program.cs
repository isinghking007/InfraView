using InfraViewAPI.Controllers;
using InfraViewAPI.Interfaces;
using InfraViewAPI.Logic;
using InfraViewAPI.Model;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register memory stores
var connectedDevices = new ConcurrentDictionary<string, DeviceInfo>();
var deviceLogs = new ConcurrentBag<DeviceLog>();
var machineLogic = new MachineLogic();
builder.Services.AddSingleton<MachineLogic>();
var watcher = new DeviceWatcher(connectedDevices, deviceLogs, machineLogic);
watcher.Start();
builder.Services.AddSingleton(connectedDevices);
builder.Services.AddSingleton(deviceLogs);


//Add Services
builder.Services.AddScoped<IMachine, MachineLogic>();
builder.Services.AddScoped<IDevice, DeviceLogic>();
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

app.Run();
