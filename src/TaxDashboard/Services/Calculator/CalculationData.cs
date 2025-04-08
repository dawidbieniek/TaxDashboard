namespace TaxDashboard.Services.Calculator;

public class CalculationData
{
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
    public LumpSumRate Rate { get; set; }
    public ContributionVariant ContributionVariant { get; set; }
}