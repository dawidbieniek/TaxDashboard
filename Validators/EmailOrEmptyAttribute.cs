using System.ComponentModel.DataAnnotations;

namespace TaxDashboard.Validators;

// Based on System.ComponentModel.DataAnnotations.EmailAttribute
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class EmailOrEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is not string valueAsString)
            return false;

        if (string.IsNullOrEmpty(valueAsString))
            return true;

        if (valueAsString.AsSpan().ContainsAny('\r', '\n'))
            return false;

        int index = valueAsString.IndexOf('@');

        return
            index > 0 &&
            index != valueAsString.Length - 1 &&
            index == valueAsString.LastIndexOf('@');
    }
}