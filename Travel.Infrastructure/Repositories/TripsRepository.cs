using Microsoft.EntityFrameworkCore;
using Travel.Application.Interfaces;
using Travel.Domain.Entities;
using Travel.Infrastructure.Context;
using Travel.Model;

namespace Travel.Infrastructure.Repositories;

public class TripsRepository : ITripsRepository
{
    TripsDbContext _context;
    public TripsRepository(TripsDbContext context)
    {
        _context = context;
    }

    public async Task<TripEntity?> GetTrip(string id)
    {
        var tripId = Guid.Parse(id);
        
        return await _context.Trips.Include(t => t.Images).FirstOrDefaultAsync(t => t.Id == tripId);
    }
    
    public List<TripEntity>  GetTrips()
    {
        return _context.Trips.ToList();
    }

    public async Task DeleteTripAsync(string id)
    {
        var tripId = Guid.Parse(id);
        var trip = _context.Trips.Find(tripId);
        if (trip != null)
        {
            _context.Trips.Remove(trip);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task CreateTrip(TripEntity trip)
    { 
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateTrip(TripEntity trip)
    {
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync();    
    }
}