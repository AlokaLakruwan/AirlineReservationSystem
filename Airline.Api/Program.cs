using Microsoft.EntityFrameworkCore;
using Airline.Data;
using Airline.Data.Repositories;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AirlineDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Airline.Api")
    )
);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAirplaneRepository, AirplaneRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAirplaneConfigurationRepository, AirplaneConfigurationRepository>();
builder.Services.AddScoped<IAirplaneLocationRepository, AirplaneLocationRepository>();

// Add controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Option A: Ignore cycles entirely (recommended for simple APIs)
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    // Optionally, you can raise the max depth if needed:
    // options.JsonSerializerOptions.MaxDepth = 64;
});

// Add OpenAPI
builder.Services.AddOpenApi();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy
          .WithOrigins("http://localhost:4200")   // <-- allow Angular app origin
          .AllowAnyHeader()                       // <-- allow any request header
          .AllowAnyMethod();                      // <-- allow GET, POST, PUT, DELETE, etc.
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(o =>
        o.WithTheme(ScalarTheme.DeepSpace)
    );
}

app.UseCors("AllowAngularDev");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();