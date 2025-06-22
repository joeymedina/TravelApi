using Travel.Model;

namespace Travel.Api.DTOs;

public class PatchTripImageDto
{
    public string? Url { get; set; }
    public string? Caption { get; set; }
    public Guid? TripId { get; set; }
}