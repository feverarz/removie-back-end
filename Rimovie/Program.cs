using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rimovie.Middlewares;
using Rimovie.Repository;
using Rimovie.Repository.Dapper;
using Rimovie.Services.AuthService;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuración de servicios
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<FilmRepository>();
builder.Services.AddScoped<RatingRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<WishListFilmRepository>();
builder.Services.AddScoped<WishListRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"])
            )
        };
    });



builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rimovie", Version = "v1" });
});

var app = builder.Build();

// 🔹 Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rimovie v1"));
}
// 🔹 Middleware de excepciones personalizado
app.UseMiddleware<ExceptionMiddleware>();


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("DATABASE_URL:");
Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE_URL") ?? "NO DEFINIDA");

Console.WriteLine("Jwt__Issuer:");
Console.WriteLine(builder.Configuration["Jwt:Issuer"] ?? "NO DEFINIDA");

app.Run();