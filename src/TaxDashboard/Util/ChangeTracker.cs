namespace TaxDashboard.Util;

internal class ChangeTracker<T>
{
    public T LastValue { get; private set; } = default!;
    private T _oldValue = default!;

    public bool HasChanged(T value)
    {
        bool hasChanged = !EqualityComparer<T>.Default.Equals(_oldValue, value);
        if (hasChanged)
        {
            LastValue = _oldValue;
            _oldValue = value;
        }
        return hasChanged;
    }
}