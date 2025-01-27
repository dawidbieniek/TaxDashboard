namespace TaxDashboard.Models.Entities;

public class Income : Entity
{
    public Client Client { get; set; } = default!;
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
}