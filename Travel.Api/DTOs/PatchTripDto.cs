using Travel.Model;

namespace Travel.Api.DTOs;

public class PatchTripDto
{
    public string? Title { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Story { get; set; } // Could be Markdown or HTML
    public List<TripImage>? Images { get; set; }
}