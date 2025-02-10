namespace TaxDashboard.Components.Pages.Dashboard.Model;

public class IncomeData(int month, decimal amount)
{
    public IncomeData(int month) : this(month, 0) { }
    public int Month { get; set; } = month;
    public decimal Amount { get; set; } = amount;
}