using System.ComponentModel;

namespace TaxDashboard.Services.Emails;
public enum EmailTemplateType
{
    [Description("Blisko limitu kasy fiskalnej")]
    AmountFiscal,
    [Description("Blisko limitu VAT")]
    AmountVAT,
    [Description("Blisko końca ulgi na start")]
    TimeStart,
    [Description("Blisko końca preferencji (Pełny)")]
    TimePreferentialFull,
    [Description("Blisko końca preferencji (Pełny)")]
    TimePreferentialPlus,
    [Description("Przekroczenie limitu kasy fiskalnej")]
    AmountFiscalDanger,
    [Description("Przekroczenie limitu VAT")]
    AmountVATDanger,
    [Description("Przekroczenie końca ulgi na start")]
    TimeStartDanger,
    [Description("Przekroczenie końca preferencji (Pełny)")]
    TimePreferentialFullDanger,
    [Description("Przekroczenie końca preferencji (Pełny)")]
    TimePreferentialPlusDanger,
}
