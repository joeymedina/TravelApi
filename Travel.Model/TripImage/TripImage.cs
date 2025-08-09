namespace Travel.Model.TripImage;

public class TripImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; }
    public string Caption { get; set; }
    public Guid TripId { get; set; }
    public Trip.Trip Trip { get; set; }   // Navigation property
}