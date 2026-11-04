using DS.UserService.api;
using DS.UserService.clients;
using DS.UserService.services;
using DS_Application.UserService.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UserDb")
    ));

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddHttpClient<IDeviceClient, DeviceHttpClient>(client =>
{
    var deviceServiceUrl = builder.Configuration["Services:DeviceServiceUrl"] ?? "http://localhost:5228";
    client.BaseAddress = new Uri(deviceServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});
builder.Services.AddHttpClient<IAuthClient, AuthHttpClient>(client =>
{
    var authServiceUrl = builder.Configuration["Services:AuthServiceUrl"] ?? "http://localhost:5000";
    client.BaseAddress = new Uri(authServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapUsersEndpoints();
app.Run();
