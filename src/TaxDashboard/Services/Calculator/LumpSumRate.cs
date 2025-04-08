using System.ComponentModel;

namespace TaxDashboard.Services.Calculator;

public enum LumpSumRate
{
    [Description("2%")]
    Rate2,
    [Description("3%")]
    Rate3,
    [Description("5,5%")]
    Rate5_5,
    [Description("8,5%")]
    Rate8_5,
    [Description("10%")]
    Rate10,
    [Description("12%")]
    Rate12,
    [Description("14%")]
    Rate14,
    [Description("15%")]
    Rate15,
    [Description("17%")]
    Rate17
}

public static class LumpSumRateExtensions
{
    public static decimal AsPercentageValue(this LumpSumRate lumpSumRate) => lumpSumRate switch
    {
        LumpSumRate.Rate2 => 0.02M,
        LumpSumRate.Rate3 => 0.03M,
        LumpSumRate.Rate5_5 => 0.055M,
        LumpSumRate.Rate8_5 => 0.085M,
        LumpSumRate.Rate10 => 0.1M,
        LumpSumRate.Rate12 => 0.12M,
        LumpSumRate.Rate14 => 0.14M,
        LumpSumRate.Rate15 => 0.15M,
        LumpSumRate.Rate17 => 0.17M,
        _ => 0
    };
}