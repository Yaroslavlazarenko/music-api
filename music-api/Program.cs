using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using music_api.Settings;
using music_api.Contexts;
using FluentValidation;
using FluentValidation.AspNetCore;
using music_api.Entities;
using Microsoft.AspNetCore.Identity;
using music_api.ExceptionHandlers;
using music_api.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register exception handlers
builder.Services.AddExceptionHandler<UserValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GenreValidationExceptionHandler>();
builder.Services.AddExceptionHandler<PlaylistValidationExceptionHandler>(); 
builder.Services.AddExceptionHandler<PerformerValidationExceptionHandler>();
builder.Services.AddExceptionHandler<SongValidationExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<ServerExceptionsHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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

var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Secret"]!));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = secret
    };
});

builder.Services.AddAuthorization();

// AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<MusicDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DB")));

// Add ASP.NET Core Identity
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<MusicDbContext>()
    .AddDefaultTokenProviders();

// AutoMapper: регистрируем все профили в сборке
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// MediatR: регистрируем все Handler'ы
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// FluentValidation: регистрируем все валидаторы
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();