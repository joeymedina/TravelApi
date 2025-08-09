namespace Travel.Model.Trip;

public class TripCreated
{
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Story { get; set; }
}