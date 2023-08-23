using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text.Json.Serialization;
using TravelPlatform.Handler;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;

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

// File upload service
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileUploadHandler, FileUploadHandler>();

// WebSocket
builder.Services.AddScoped<TravelPlatform.Handler.WebSocketHandler>();
builder.Services.AddScoped<TravelPlatform.Handler.WebSocketManager>();

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

// WebSocket
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/ws", out var remaining))
    {
        var channelName = remaining.Value.Trim('/'); // 取得通道名稱

        if (!string.IsNullOrEmpty(channelName))
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using (WebSocket ws = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var wsHandler = context.RequestServices.GetRequiredService<WebSocketHandler>();
                    await wsHandler.HandleWebSocketAsync(channelName, ws);
                }
            }
            else
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        }
    }
    else await next();
});

app.Run();
