using BCrypt.Net;
using FluentValidation;
using FluentValidation.AspNetCore;
using IST.Zeiterfassung.API.Middleware;
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
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Datenbank (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// HTTP Client für Feiertagsservice
builder.Services.AddHttpClient<IFeiertagsService, FeiertagsService>();

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();
builder.Services.AddScoped<IZeitmodellRepository, ZeitmodellRepository>();
builder.Services.AddScoped<IFeiertagRepository, FeiertagRepository>();
builder.Services.AddScoped<FeiertagsImportService>();
builder.Services.AddScoped<ISettingsService, SettingsService>(); // 🔧 HINZUFÜGEN!
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // bestehend:
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // ➕ NEU: JWT-Auth-Schema ergänzen
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Gib hier deinen gültigen JWT ein: Bearer <token>"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// CORS – Muss vor Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();

    });
});



// JWT Auth
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwtSettings = jwtSection.Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

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

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

// Admin-Account anlegen
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
app.UseMiddleware<ApiLoggingMiddleware>();
app.UseAuthentication();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseDeveloperExceptionPage();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IST Zeiterfassung API v1");
});
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();
