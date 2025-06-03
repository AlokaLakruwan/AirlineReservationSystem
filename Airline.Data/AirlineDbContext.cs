using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
        : base(options)
    { }

    public DbSet<Role> Role { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Airport> Airport { get; set; }
    public DbSet<Airplane> Airplane { get; set; }
    public DbSet<AirplaneConfiguration> AirplaneConfiguration { get; set; }
    public DbSet<AirplaneLocation> AirplaneLocation { get; set; }
    public DbSet<Flight> Flight { get; set; }
    public DbSet<Booking> Booking { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Airport>().ToTable("Airport");
        modelBuilder.Entity<Airplane>().ToTable("Airplane");
        modelBuilder.Entity<AirplaneConfiguration>().ToTable("AirplaneConfiguration");
        modelBuilder.Entity<AirplaneLocation>().ToTable("AirplaneLocation");
        modelBuilder.Entity<Flight>().ToTable("Flight");
        modelBuilder.Entity<Booking>().ToTable("Booking");

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Airport>()
            .HasMany(a => a.DepartingFlights)
            .WithOne(f => f.OriginAirport)
            .HasForeignKey(f => f.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Airport>()
            .HasMany(a => a.ArrivingFlights)
            .WithOne(f => f.DestinationAirport)
            .HasForeignKey(f => f.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Airplane>()
            .HasMany(a => a.Configurations)
            .WithOne(c => c.Airplane)
            .HasForeignKey(c => c.AirplaneId);

        modelBuilder.Entity<Airplane>()
            .HasMany(a => a.Flights)
            .WithOne(f => f.Airplane)
            .HasForeignKey(f => f.AirplaneId);

        modelBuilder.Entity<Airplane>()
            .HasOne(a => a.Location)
            .WithOne(l => l.Airplane)
            .HasForeignKey<AirplaneLocation>(l => l.AirplaneId);

        modelBuilder.Entity<AirplaneLocation>()
            .HasOne(l => l.Airplane)
            .WithOne(a => a.Location)
            .HasForeignKey<AirplaneLocation>(l => l.AirplaneId);

        modelBuilder.Entity<AirplaneLocation>()
            .HasOne(l => l.Airport)
            .WithMany(a => a.AirplaneLocations)
            .HasForeignKey(l => l.CurrentAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasMany(f => f.Bookings)
            .WithOne(b => b.Flight)
            .HasForeignKey(b => b.FlightId);

        modelBuilder.Entity<Airplane>()
            .HasCheckConstraint("CHK_Airplane_CapacityClass",
                "CapacityClass IN ('Small','Medium','Large')");

        modelBuilder.Entity<AirplaneConfiguration>()
            .HasCheckConstraint("CHK_ApConfig_ClassName",
                "ClassName IN ('First','Business','Economy')");

        modelBuilder.Entity<Booking>()
            .HasCheckConstraint("CHK_Booking_ServiceClass",
                "ServiceClass IN ('First','Business','Economy')");

        modelBuilder.Entity<Booking>()
            .HasIndex(b => new { b.FlightId, b.SeatNumber }).IsUnique();
    }
}
