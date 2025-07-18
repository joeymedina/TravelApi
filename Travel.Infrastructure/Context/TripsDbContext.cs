using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.Infrastructure.Context;

public class TripsDbContext : DbContext
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripImage> TripImages { get; set; }

    public TripsDbContext(DbContextOptions<TripsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripImage>()
            .HasOne(ti => ti.Trip)
            .WithMany(t => t.Images)
            .HasForeignKey(ti => ti.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}