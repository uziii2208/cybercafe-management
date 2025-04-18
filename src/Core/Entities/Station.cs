namespace Core.Entities;

public class Station : BaseEntity
{
    public int StationNumber { get; set; }
    public string HardwareSpecification { get; set; } = string.Empty;
    public StationStatus Status { get; set; }
    public decimal HourlyRate { get; set; }
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}

public enum StationStatus
{
    Free,
    Occupied,
    Maintenance
}