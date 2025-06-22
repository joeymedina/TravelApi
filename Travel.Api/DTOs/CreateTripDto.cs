namespace Travel.Api.DTOs;

public class CreateTripDto
{
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Story { get; set; }
}