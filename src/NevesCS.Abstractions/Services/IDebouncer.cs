namespace NevesCS.Abstractions.Services;

/// <summary>
/// Used to ensure a specified action is only called once after a specified period of inactivity.
///
/// </summary>
public interface IDebouncer<TInAction> : IDisposable
{
    public void Initialize(Action<TInAction> action, int millisecondDelay);

    public void Trigger(TInAction value);
}
