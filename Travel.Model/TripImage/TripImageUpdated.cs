namespace Travel.Model.TripImage;

public class TripImageUpdated
{
    public string? Url { get; set; }
    public string? Caption { get; set; }
    public Guid? TripId { get; set; }
}