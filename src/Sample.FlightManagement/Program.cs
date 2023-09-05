using FlightManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./settings/common.json", true);
builder.Configuration.AddJsonFile("./settings/flightmanagement.json", true);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.AddResourceConverter(true);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<FlightManagementDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddHateoas();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<FlightManagementDbContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
