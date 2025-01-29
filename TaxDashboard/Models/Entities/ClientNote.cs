namespace TaxDashboard.Models.Entities;

public class ClientNote : Entity
{
    public Client Client { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
}