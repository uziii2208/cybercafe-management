namespace Core.Entities;

public class ServiceOrder : BaseEntity
{
    public int SessionId { get; set; }
    public virtual Session Session { get; set; } = null!;
    
    public int ServiceId { get; set; }
    public virtual Service Service { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled
}