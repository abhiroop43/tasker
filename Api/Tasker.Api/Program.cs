var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Host);

var app = builder.Build();

app.UseApiServices();

await app.RunAsync();
