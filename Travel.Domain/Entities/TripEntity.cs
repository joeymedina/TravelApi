namespace Travel.Domain.Entities;

public class TripEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Story { get; set; } // Could be Markdown or HTML
    public List<TripImageEntity> Images { get; set; } = [];
}