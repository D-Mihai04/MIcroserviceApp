using DS.UserService.services;
using DS_Application.UserService.data;
using Microsoft.EntityFrameworkCore;
using DS.UserService.api;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UserDb")
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUserRepo, UserRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapUsersEndpoints();

app.Run();