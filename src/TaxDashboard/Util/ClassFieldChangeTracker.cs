using System.Collections.Immutable;

namespace TaxDashboard.Util;

internal class ClassFieldChangeTracker<T> where T : class
{
    private readonly List<Func<T, object?>> _fieldSelectors;
    private readonly Dictionary<Func<T, object?>, object?> _oldValues = [];

    public ClassFieldChangeTracker(params List<Func<T, object?>> fieldSelectors)
    {
        _fieldSelectors = fieldSelectors;

        foreach (var selector in _fieldSelectors)
            _oldValues[selector] = default;
    }

    public ImmutableDictionary<Func<T, object?>, object?> LastValues => _oldValues.ToImmutableDictionary();

    public bool HasChanged(T? value)
    {
        bool hasChanged = false;

        foreach (var selector in _fieldSelectors)
        {
            object? newValue = value is not null ? selector(value) : null;

            if (!_oldValues.TryGetValue(selector, out object? oldValue) || !Equals(oldValue, newValue))
            {
                _oldValues[selector] = newValue;
                hasChanged = true;
            }
        }

        return hasChanged;
    }
}