using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using TravelPlatform.Handler.Facebook;
using TravelPlatform.Handler.File;
using TravelPlatform.Handler.Token;
using TravelPlatform.Hubs;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services.ChatRoom;
using TravelPlatform.Services.Facebook;
using TravelPlatform.Services.File.FileUpload;
using TravelPlatform.Services.File.Storage;
using TravelPlatform.Services.Response;
using TravelPlatform.Services.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to SQL Server
DotNetEnv.Env.Load();
builder.Services.AddDbContext<TravelContext>(options =>
        options.UseSqlServer(Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE__CONNECTIONSTRING")));

// load .env file
builder.Configuration.AddEnvironmentVariables();

// SignalR
builder.Services.AddSignalR();

// S3
builder.Services.AddScoped<IStorageService, StorageService>();

/*
 ======================
 ===    Service     ===
 ======================
 */

// Response service
builder.Services.AddScoped<IResponseService, ResponseService>();

// File upload service
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileUploadHandler, FileUploadHandler>();

// Token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();

// Facebook service
builder.Services.AddScoped<IFacebookService, FacebookService>();
builder.Services.AddScoped<IFacebookHandler, FacebookHandler>();

// Chat service
builder.Services.AddScoped<IChatService, ChatService>();

/*
 ======================
 ===   ./Service    ===
 ======================
 */

// App Api Version
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// SignalR
app.MapHub<ClientChatHub>("/hubs/ChatHub");

app.Run();
