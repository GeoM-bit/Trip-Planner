using Microsoft.EntityFrameworkCore;
using TripPlanner.Context;
using TripPlanner.Controllers.Mappers;
using TripPlanner.Logic.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<TripPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TripPlannerConnectionString") ?? throw new InvalidOperationException("Connection string 'TripPlannerConnectionString' not found.")));

// Dependency Injection for Repositories
builder.Services.AddScoped<IBusinessTripRequestRepository, BusinessTripRequestRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(BusinessTripRequestProfile));

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
