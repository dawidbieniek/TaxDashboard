using TaxDashboard.Services.Calculator;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TaxDashboard.Services;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class CalculatorService
{
    private const decimal PreferentialPensionContribution = 273.24M;
    private const decimal FullZusPensionContribution = 1_015.78M;
    private const decimal PreferentialDisabilityContribution = 111.98M;
    private const decimal FullZusDisabilityContribution = 416.3M;
    private const decimal PreferentialIllnessContribution = 23.30M;
    private const decimal FullZusIllnessContribution = 127.49M;
    private const decimal PreferentialAccidentContribution = 23.38M;
    private const decimal FullZusAccidentContribution = 86.9M;
    private const decimal FullZusWorkFund = 127.49M;

    private const decimal LumpSumHealthContributionBelow60k = 461.66M;
    private const decimal LumpSumHealthContributionAbove60kBelow300k = 769.43M;
    private const decimal LumpSumHealthContributionAbove300k = 1_384.97M;

    private const decimal TaxScaleHealthContributionMonthlyThreshold = 3499.5M;
    private const decimal TaxScaleHealthContributionThresholdPercentageValue = 0.09M;
    private const decimal TaxScaleFirstBracketThreshold = 120_000M;
    private const decimal TaxScaleFirstBracketPercentageValue = 0.12M;
    private const decimal TaxScaleSecondBracketPercentageValue = 0.32M;
    private const decimal TaxScaleReductionThreshold = 30_000M;
    private const decimal TaxScaleReductionAmount = 3_600M;

    private const decimal LinearTaxHealtContributionMonthlyThreshold = 6_427.75M;
    private const decimal LinearTaxHealtContributionPercentageValue = 0.049M;
    private const decimal LinearTaxHealthReductionThreshold = 12_900M;
    private const decimal LinearTaxPercentageValue = 0.19M;

    private const decimal FirstBracketHealthContributionMonthlyValue = 314.96M;

    public static decimal CalculateSocialContribution(CalculationData data)
    {
        return data.ContributionVariant switch
        {
            ContributionVariant.Preferential => PreferentialPensionContribution + PreferentialDisabilityContribution + PreferentialAccidentContribution,
            ContributionVariant.PreferentialWithIllness => PreferentialPensionContribution + PreferentialDisabilityContribution + PreferentialAccidentContribution + PreferentialIllnessContribution,
            ContributionVariant.FullZUS => FullZusPensionContribution + FullZusDisabilityContribution + FullZusAccidentContribution + FullZusWorkFund,
            ContributionVariant.FullZUSWithIllness => FullZusPensionContribution + FullZusDisabilityContribution + FullZusAccidentContribution + FullZusWorkFund + FullZusIllnessContribution,
            _ => 0,
        } * 12;
    }

    public static (decimal Tax, decimal HealthContribution, decimal SocialContribution) CalculateLumpSumValues(CalculationData data)
    {
        decimal socialContribution = CalculateSocialContribution(data);

        decimal healthContributionBaseValue = Math.Max(data.Income - socialContribution, 0);
        decimal healthContribution = GetLumpSumHealthContribution(healthContributionBaseValue);

        decimal healthContributionToReduce = healthContribution / 2;
        decimal taxBaseValue = data.Income - healthContributionToReduce - socialContribution;
        decimal tax = Math.Max(taxBaseValue * data.Rate.AsPercentageValue(), 0);

        return (tax, healthContribution, socialContribution);
    }

    public static (decimal Tax, decimal HealthContribution, decimal SocialContribution) CalculateTaxScaleValues(CalculationData data)
    {
        decimal socialContribution = CalculateSocialContribution(data);

        decimal net = data.Income - data.Expenses;
        decimal baseValue = Math.Max(net - socialContribution, 0);

        decimal healthContribution = baseValue > TaxScaleHealthContributionMonthlyThreshold * 12
            ? baseValue * TaxScaleHealthContributionThresholdPercentageValue
            : FirstBracketHealthContributionMonthlyValue * 12;

        decimal tax12 = Math.Min(baseValue * TaxScaleFirstBracketPercentageValue, TaxScaleFirstBracketThreshold * TaxScaleFirstBracketPercentageValue);
        decimal tax32 = baseValue > TaxScaleFirstBracketThreshold
            ? (baseValue - TaxScaleFirstBracketThreshold) * TaxScaleSecondBracketPercentageValue
            : 0;
        decimal reduction = baseValue < TaxScaleReductionThreshold
            ? baseValue * TaxScaleFirstBracketPercentageValue
            : TaxScaleReductionAmount;
        decimal tax = Math.Max(tax12 + tax32 - reduction, 0);

        return (tax, healthContribution, socialContribution);
    }

    public static (decimal Tax, decimal HealthContribution, decimal SocialContribution) CalculateLinearTaxValues(CalculationData data)
    {
        decimal socialContribution = CalculateSocialContribution(data);

        decimal net = data.Income - data.Expenses;
        decimal healthContributionBaseValue = Math.Max(net - socialContribution, 0);
        decimal healthContribution = healthContributionBaseValue > LinearTaxHealtContributionMonthlyThreshold * 12
            ? healthContributionBaseValue * LinearTaxHealtContributionPercentageValue
            : FirstBracketHealthContributionMonthlyValue * 12;

        decimal healthContributionReduction = Math.Min(healthContribution, LinearTaxHealthReductionThreshold);
        decimal taxBaseValue = net - socialContribution - healthContributionReduction;
        decimal tax = Math.Max(taxBaseValue * LinearTaxPercentageValue, 0);

        return (tax, healthContribution, socialContribution);
    }

    private static decimal GetLumpSumHealthContribution(decimal baseValue)
    {
        return baseValue < 60_000
            ? LumpSumHealthContributionBelow60k * 12
            : baseValue < 300_000
                ? LumpSumHealthContributionAbove60kBelow300k * 12
                : LumpSumHealthContributionAbove300k * 12;
    }
}