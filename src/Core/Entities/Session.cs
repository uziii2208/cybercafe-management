namespace Core.Entities;

public class Session : BaseEntity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    
    public int StationId { get; set; }
    public virtual Station Station { get; set; } = null!;
    
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal TotalCost { get; set; }
    
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}