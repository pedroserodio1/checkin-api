using Checkin.Api.Data;
using Checkin.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Checkin.Api.Extensions;
using Checkin.Api.Common;
using Checkin.Api.Utils;
using Checkin.Api.Auth;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(builder.Environment.EnvironmentName);

// Adiciona serviços
builder.Services.AddControllers();         // Para usar controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **Registrar o Service**
builder.Services.AddServices();

// Configura o DbContext com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// seta as configurações de segurança
builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("SecuritySettings"));

//JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

//inicia um passwordhasher com as configurações de segurança
builder.Services.AddSingleton<PasswordHasher>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(
                JsonNamingPolicy.CamelCase, true // case-insensitive
            )
        ));

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
