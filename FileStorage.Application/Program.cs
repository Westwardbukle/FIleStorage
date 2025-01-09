using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FileStorage.Application.Extensions;
using FileStorage.Common.Options;
using FileStorage.Database.Context;
using System.Reflection;
using FileStorage.Common.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var apiTitle = $"{builder.Configuration.GetValue<string>("Application:AppTitle")} - FileStorage API";

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.RegisterServices();
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var mapperConfig = new MapperConfiguration(mc => { });

var mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddCors(opt =>
{
    var origins = Environment.GetEnvironmentVariable("ORIGINS");
    opt.AddPolicy("CorsPolicy",
        configure => configure
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithOrigins(string.IsNullOrEmpty(origins) ? builder.Configuration.GetSection("Origins").Get<string[]>() : origins.Split(","))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .Build());
});

builder.Services.AddDbContext<CommonContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("FILES_DB") ?? string.Empty;

    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = Environment.GetEnvironmentVariable("DEFAULT_DB") ?? string.Empty;
    }
    
    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = builder.Configuration.GetConnectionString("Files") ?? string.Empty;
    }

    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
    
    options.UseNpgsql(connectionString);
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "FileStorage.Domain.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = apiTitle });
    options.DescribeAllParametersInCamelCase();
});

var app = builder.Build();

app.UseCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseCors("CorsPolicy");

app.MapControllers();

if (app.Services.CreateScope().ServiceProvider.GetService(typeof(CommonContext)) is CommonContext dbContext)
{
    dbContext.Database.Migrate();
}

app.Run();