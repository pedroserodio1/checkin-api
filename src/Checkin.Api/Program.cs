using Checkin.Api.Data;
using Checkin.Api.Services;
using Microsoft.EntityFrameworkCore;
using Checkin.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços
builder.Services.AddControllers();         // Para usar controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **Registrar o Service**
builder.Services.AddServices();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Mapeia os controllers

app.Run();
