using System.Text;
using System.Threading.RateLimiting;
using ChatZone.Core.Notifications;
using ChatZone.Features.Identity.Authentication.Login;
using ChatZone.Features.QuickMessages.Create;
using ChatZone.Matchmaking;
using ChatZone.Shared.Context;
using ChatZone.Shared.Hubs;
using ChatZone.Shared.Middlewares;
using ChatZone.Shared.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args); //zwraca obiekt i sluzy dla konfiguracji webowej
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var requestLimit = builder.Configuration.GetValue<int>("RateLimiting:RequestLimit", 50);
var requestInSeconds = builder.Configuration.GetValue<int>("RateLimiting:RequestInSeconds", 60);
var queueLimit = builder.Configuration.GetValue<int>("RateLimiting:QueueLimit", 2);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.AddFixedWindowLimiter("RateLimitingPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = requestLimit;
        limiterOptions.Window = TimeSpan.FromSeconds(requestInSeconds);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = queueLimit;
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
app.UseRateLimiter();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatZoneHub>("/chat").RequireAuthorization();

app.MapControllers().RequireRateLimiting("RateLimitingPolicy"); //with 'builder.Services.AddControllers();'

app.Run();