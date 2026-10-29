var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "DS Application is running");

app.MapGet("/healthcheck", () => true);

app.Run();