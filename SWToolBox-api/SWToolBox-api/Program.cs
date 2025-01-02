using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWToolBox_api.Database;
using SWToolBox_api.Features.Guilds.Admin.Authorizations;
using SWToolBox_api.Features.Guilds.Authorizations;
using SWToolBox_api.Features.Guilds.ManageMembers.Authorization;
using SWToolBox_api.Features.Players.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddDbContextPool<SwDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("db")));

builder.Services.AddAuthorizationBuilder()
    .AddManagePlayerDataPolicy()
    .AddReadGuildDataPolicy()
    .AddManageMembersPolicy()
    .AddChangeRankPolicy()
    .AddManageDefensesPolicy()
    .AddGuildAdminPolicy();

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
    
    config.Errors.UseProblemDetails();
}).UseSwaggerGen();

app.Run();
