namespace Travel.Model.TripImage;

public class TripImage
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Caption { get; set; }
    public Guid TripId { get; set; }
}