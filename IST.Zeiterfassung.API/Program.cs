using BCrypt.Net;
using FluentValidation;
using FluentValidation.AspNetCore;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Settings;
using IST.Zeiterfassung.Application.Validators;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using IST.Zeiterfassung.Persistence;
using IST.Zeiterfassung.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Datenbank-Konfiguration (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Feiertage abrufen und lokal cachen
builder.Services.AddHttpClient<IFeiertagsService, FeiertagsService>();


// Dependency Injection für Services & Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFeiertagRepository, FeiertagRepository>(); // ✅ HIER NEU
builder.Services.AddScoped<FeiertagsImportService>();
builder.Services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();

// Controller + FluentValidation registrieren
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

builder.Services.AddControllers(); // 

// Swagger für OpenAPI Dokumentation
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});





// JWT-Konfiguration (muss VOR builder.Build() passieren)
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience
    };
});

builder.Services.AddAuthorization(); // ✅ HIER ergänzen
var app = builder.Build();

// Admin-Benutzer beim Start anlegen
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

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IST Zeiterfassung API v1");
});

app.MapControllers();
app.Run();
