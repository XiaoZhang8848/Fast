namespace Fast.Infrastructure.Entities;

public abstract class AuditedEntity : Entity
{
    public long CreateId { get; set; }
    public DateTime CreateTime { get; set; }
    public long? UpdateId { get; set; }
    public DateTime? UpdateTime { get; set; }
    public long? DeleteId { get; set; }
    public DateTime? DeleteTime { get; set; }
    public bool IsEnable { get; set; }
}