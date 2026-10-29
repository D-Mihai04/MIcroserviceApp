using DS_Application.DeviceService.data;
using DS.DeviceService.endpoints;
using DS.DeviceService.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DeviceContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DeviceDb")
    ));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IDeviceRepo, DeviceRepository>();
builder.Services.AddScoped<IDeviceServices, DeviceServices>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapDeviceEndpoints();

app.Run();