using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWToolBox_api.Database;
using SWToolBox_api.Features.Guilds.Authorization;
using SWToolBox_api.Features.Players.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddDbContextPool<SwDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("db")));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ManagePlayerData", policy =>
    {
        policy.Requirements.Add(new ManagePlayerDataRequirement());
    })
    .AddPolicy("ReadGuildData", policy =>
    {
        policy.Requirements.Add(new ReadGuildDataRequirement());
    });

var key = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!);
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidAudience = builder.Configuration["Authentication:ValidAudience"],
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
    };
    
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"Challenge issued: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddTransient<IAuthorizationHandler, ManagePlayerAuthorizationHandler>();
builder.Services.AddTransient<IAuthorizationHandler, ReadGuildAuthorizationHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Release 1";
            s.Title = "My API";
            s.Version = "v1";
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.DefaultVersion = 1;
    config.Versioning.PrependToRoute = true;
}).UseSwaggerGen();

app.Run();
