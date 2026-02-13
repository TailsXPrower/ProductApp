using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Configuration;
using ProductApp.Application.Contracts;
using ProductApp.Infrastructure.Persistence;
using ProductApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Database
if (!builder.Environment.IsEnvironment("Test"))
    builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "productapp");

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Telemetry, Health Checks, etc.
builder.AddServiceDefaults();

// FastEndpoints and Swagger
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerDocument();

// Validators
builder.Services.SetupValidators();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.MapDefaultEndpoints();

app.Run();
