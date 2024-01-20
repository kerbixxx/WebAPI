using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SimbirSoft.Data;
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
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<IAnimalTypeRepository, AnimalTypeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();




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
