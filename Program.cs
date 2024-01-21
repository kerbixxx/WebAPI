using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SimbirSoft.Data;
using SimbirSoft.Middleware;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Implementations;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Implementations;
using SimbirSoft.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SQLite");
    //options.UseNpgsql(connectionString);
    options.UseSqlite(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IAnimalTypeService, AnimalTypeService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();

builder.Services.AddScoped<IAnimalTypeRepository, AnimalTypeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IVisitedLocationRepository, VisitedLocationRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Input your username and password to access this API",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                },
                Scheme = "Basic",
                Name = "Authorization",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
