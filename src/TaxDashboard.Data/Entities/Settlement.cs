using LifeManagers.Data;

namespace TaxDashboard.Data.Entities;

public class Settlement : Entity
{
    public Client Client { get; set; } = default!;
    public DateOnly ContextDate { get; set; }
}