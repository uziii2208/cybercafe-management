namespace Core.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public ServiceCategory Category { get; set; }
    public virtual ICollection<ServiceOrder> Orders { get; set; } = new List<ServiceOrder>();
}

public enum ServiceCategory
{
    Food,
    Beverage,
    Peripheral,
    Other
}