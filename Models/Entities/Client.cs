using System.ComponentModel.DataAnnotations.Schema;

using TaxDashboard.Models.Enums;

namespace TaxDashboard.Models.Entities;

public class Client : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public required DateTime JoinDateTime { get; set; }
    public string NIP { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool Suspended { get; set; } = false;

    public bool UseCashRegister { get; set; } = false;
    public string Subscription { get; set; } = string.Empty;
    public required Bank Bank { get; set; }
    public bool EmploymentContract { get; set; } = false;
    public bool VAT { get; set; } = false;
    public bool CashMethod { get; set; } = false;
    public TaxType TaxType { get; set; } = TaxType.Scale;
    public ReductionType ReductionType { get; set; } = ReductionType.Start;
    public decimal AuthorizationPrice { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public PaymentType PITPaymentType { get; set; } = PaymentType.Monthly;
    public PaymentType VATPaymentType { get; set; } = PaymentType.Monthly;

    public bool VATRHandled { get; set; } = false;
    public bool CEIDG1Handled { get; set; } = false;
    public bool ZUSHandled { get; set; } = false;
    public bool Invoiced { get; set; } = false;
    public DateTime? ZUSDraHandledDate { get; set; } = default;
    public DateTime? TaxHandledDate { get; set; } = default;
    public List<JPKV7> JPKV7HandledDates { get; set; } = [];
    public List<VATUE> VATUEHandledDates { get; set; } = [];

    public List<InvoiceCount> Invoices { get; set; } = [];
    public List<Income> Incomes { get; set; } = [];
    public List<ClientNote> Notes { get; set; } = [];

    [NotMapped]
    public string FullName => $"{Name} {Surname}";
}