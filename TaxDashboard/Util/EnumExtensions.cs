using System.ComponentModel;
using System.Reflection;

namespace TaxDashboard.Util;

public static class EnumExtensions
{
    /// <summary>
    /// Returns text written in <see cref="DescriptionAttribute"/>. Use this attribute to decorate
    /// enum value
    /// </summary>
    public static string? GetDescriptor<T>(this T enumValue) where T : Enum
    {
        FieldInfo? info = enumValue.GetType().GetField(enumValue.ToString());

        if (info is null)
            return null;

        DescriptionAttribute? attr = info.GetCustomAttribute<DescriptionAttribute>(false);

        return attr?.Description ?? enumValue.ToString();
    }
}