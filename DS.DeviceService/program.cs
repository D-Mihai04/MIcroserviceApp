using DS_Application.DeviceService.data;
using DS.DeviceService.clients;
using DS.DeviceService.endpoints;
using DS.DeviceService.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DeviceContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DeviceDb")
    ));

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories & Services
builder.Services.AddScoped<IDeviceRepo, DeviceRepository>();
builder.Services.AddScoped<IDeviceServices, DeviceServices>();

// HTTP Client to UserService
builder.Services.AddHttpClient<IUserClient, UserHttpClient>(client =>
{
    var userServiceUrl = builder.Configuration["Services:UserServiceUrl"] ?? "http://localhost:5068";
    client.BaseAddress = new Uri(userServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

// Automatically apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeviceContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapDeviceEndpoints();

app.Run();
