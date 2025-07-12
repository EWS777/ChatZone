using System.Text;
using ChatZone.Chat.Individual;
using ChatZone.Context;
using ChatZone.Core.Notifications;
using ChatZone.Features.Identity.Authentication.Login;
using ChatZone.Features.QuickMessages.Create;
using ChatZone.Matchmaking;
using ChatZone.Security;
using ChatZone.Validation.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args); //zwraca obiekt i sluzy dla konfiguracji webowej

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer(); //to create Swagger
builder.Services.AddSwaggerGen(); //to create Swagger
builder.Services.AddControllers(); //find all classes derived from ControllerBase which can be used
builder.Services.AddSignalR();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(CreateQuickMessageHandler).Assembly));

builder.Services.AddScoped<IToken, Token>();
builder.Services.AddScoped<IMatchmakingService, MatchmakingService>();

builder.Services.AddDbContext<ChatZoneDbContext>(opt =>
{
    string connString = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(connString);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //by who
        ValidateAudience = true, //for whom
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
     
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var tokenFromCookie = context.Request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(tokenFromCookie))
            {
                context.Token = tokenFromCookie;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        }
    };
}).AddJwtBearer("IgnoreTokenExpirationScheme", opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var refreshTokenFromCookie = context.Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                context.Token = refreshTokenFromCookie;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
var app = builder.Build();

EmailSender.EmailSettings(app.Services.GetRequiredService<IConfiguration>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chat");

app.MapControllers(); //with 'builder.Services.AddControllers();'

app.Run();