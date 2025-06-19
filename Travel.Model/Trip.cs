namespace Travel.Model;

public class Trip
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Story { get; set; } // Could be Markdown or HTML
    public ICollection<TripImage> Images { get; set; }
}