using BCrypt.Net;
using FluentValidation;
using FluentValidation.AspNetCore;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Validators;
using IST.Zeiterfassung.Persistence;
using IST.Zeiterfassung.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);

//Todo: ein globales Error-Handling (Middleware) aufbauen oder Result<T> mit HTTP-Codes zu mappen.


// Datenbank-Konfiguration (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection für Services & Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Controller + FluentValidation registrieren
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>();
    });

// Swagger für OpenAPI Dokumentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware-Pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IST Zeiterfassung API v1");
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    var username = config["DefaultAdmin:Username"];
    var email = config["DefaultAdmin:Email"];
    var password = config["DefaultAdmin:Passwort"];
    var role = config["DefaultAdmin:Role"];

    if (!context.Users.Any(u => u.Username == username))
    {
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = username!,
            Email = email!,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = Enum.TryParse<Role>(role, out var parsedRole) ? parsedRole : Role.Admin,
            Aktiv = true,
            ErstelltAm = DateTime.UtcNow
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // ← wichtig, sonst funktionieren die Routen nicht!

app.Run();
