namespace Core.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public MembershipTier MembershipTier { get; set; }
    public int MembershipPoints { get; set; }
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}