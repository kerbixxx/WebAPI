using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite("Filename = Database.db"));

builder.Services.AddTransient<IRepository<Account>, Repository<Account>>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
