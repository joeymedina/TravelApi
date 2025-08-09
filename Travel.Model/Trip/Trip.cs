namespace Travel.Model.Trip;

public class Trip
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Story { get; set; }
    public List<TripImage.TripImage> Images { get; set; }
}