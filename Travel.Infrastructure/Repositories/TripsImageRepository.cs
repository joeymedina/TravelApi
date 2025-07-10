using Microsoft.EntityFrameworkCore;
using Travel.Domain.Interfaces;
using Travel.Infrastructure.Context;
using Travel.Model;

namespace Travel.Infrastructure.Repositories;

public class TripsImageRepository(TripsDbContext context) : ITripsImageRepository
{
    TripsDbContext _context = context;
    
    public async Task<List<TripImage>?> GetTripImages(string id)
    {
        var images = _context.TripImages.Where(x => x.TripId == Guid.Parse(id));

        if (images.Any())
        {
            return await images.ToListAsync();
        }

        return null;
    }
    
    public async Task<TripImage?> GetTripImage(string tripId, string id)
    {
        var image = _context.TripImages.Where(x => x.TripId == Guid.Parse(tripId) && x.Id == Guid.Parse(id));
        if (image.Any())
        {
            return await image.FirstOrDefaultAsync();
        }
        
        return null;
    }

    public async Task CreateTripImage(TripImage tripImage)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripImage.TripId);
        if (!tripExists)
            throw new ArgumentException("Invalid TripId");
        
        await _context.TripImages.AddAsync(tripImage);
        await _context.SaveChangesAsync();
    }

    public void DeleteTripImage(Guid id)
    {
        var image = _context.TripImages.Find(id);
        if (image != null)
        {
            _context.TripImages.Remove(image);
        }
        _context.SaveChanges();   
    }
    
    public void DeleteTripImage(string id)
    {
        var tripImageId = Guid.Parse(id);
        DeleteTripImage(tripImageId);
    }
    
    public async Task UpdateTripImage(TripImage tripImage)
    {
        _context.TripImages.Update(tripImage);
        await _context.SaveChangesAsync();
    }
}