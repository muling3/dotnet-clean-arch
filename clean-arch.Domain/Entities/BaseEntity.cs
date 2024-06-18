namespace clean_arch.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string UpdatedBy { get; set; } = default!;
    public string DeletedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}