using Microsoft.EntityFrameworkCore;
using Travel.Domain.Entities;
using Travel.Model;

namespace Travel.Infrastructure.Context;

public class TripsDbContext : DbContext
{
    public DbSet<TripEntity> Trips { get; set; }
    public DbSet<TripImageEntity> TripImages { get; set; }

    public TripsDbContext(DbContextOptions<TripsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripImageEntity>()
            .HasOne(ti => ti.Trip)
            .WithMany(t => t.Images)
            .HasForeignKey(ti => ti.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}