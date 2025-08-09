namespace Travel.Domain.Entities;

public class TripImageEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; }
    public string Caption { get; set; }
    public Guid TripId { get; set; }
    public TripEntity Trip { get; set; }   // Navigation property
}