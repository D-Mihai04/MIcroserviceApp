using DS.AuthService.data;
using DS.AuthService.endpoints;
using DS.AuthService.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CredentialContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AuthDb")
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ICredentialRepo, CredentialRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddHttpClient<DS.AuthService.clients.IUserClient, DS.AuthService.clients.UserHttpClient>(client =>
{
    var userServiceUrl = builder.Configuration["Services:UserServiceUrl"] ?? "http://localhost:5068";
    client.BaseAddress = new Uri(userServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CredentialContext>();
    db.Database.Migrate();
}

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapAuthEndpoints();
app.Run();
