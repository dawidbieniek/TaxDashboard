using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TaxDashboard.Models.Enums;
using TaxDashboard.Validators;

namespace TaxDashboard.Models.Entities;

public class Client : Entity
{
    [MaxLength(64, ErrorMessage = "Imię jest zbyt długie (max 64)")]
    public string Name { get; set; } = string.Empty;
    [MaxLength(64, ErrorMessage = "Nazwisko jest zbyt długie (max 64)")]
    public string Surname { get; set; } = string.Empty;
    public required DateTime JoinDateTime { get; set; }
    [MaxLength(10, ErrorMessage = "NIP jest zbyt długi (max 10)")]
    public string NIP { get; set; } = string.Empty;
    [PhoneOrEmpty(ErrorMessage = "Nieprawidłowy numer telefonu")]
    public string PhoneNumber { get; set; } = string.Empty;
    [EmailOrEmpty(ErrorMessage = "Nieprawidłowy adres email")]
    public string Email { get; set; } = string.Empty;

    public bool Suspended { get; set; } = false;

    public bool UseCashRegister { get; set; } = false;
    [MaxLength(64, ErrorMessage = "Typ abonamenu jest zbyt długi (max 64)")]
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
    public List<Settlement> Settlements { get; set; } = [];

    [NotMapped]
    public string FullName => $"{Name} {Surname}";
}

public record NewClientData
{
    [MaxLength(64, ErrorMessage = "Imię jest zbyt długie (max 64)")]
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string Name { get; set; } = string.Empty;
    [MaxLength(64, ErrorMessage = "Nazwisko jest zbyt długie (max 64)")]
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string Surname { get; set; } = string.Empty;
    [Required(ErrorMessage = "Data założenia jest wymagana")]
    public DateTime JoinDateTime { get; set; }
    [MaxLength(10, ErrorMessage = "NIP jest zbyt długi (max 10)")]
    [Required(ErrorMessage = "NIP jest wymagany")]
    public string NIP { get; set; } = string.Empty;
    [PhoneOrEmpty(ErrorMessage = "Nieprawidłowy numer telefonu")]
    public string PhoneNumber { get; set; } = string.Empty;
    [EmailOrEmpty(ErrorMessage = "Nieprawidłowy adres email")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Przydzielenie do banku jest wymagane")]
    public Bank Bank { get; set; } = default!;
}