namespace TaxDashboard.Models.Entities;

public class Settlement : Entity
{
    public Client Client { get; set; } = default!;
    public DateOnly ContextDate { get; set; }
}