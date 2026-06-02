namespace NevesCS.Abstractions.Traits
{
    public interface IAsyncInitializableWithOptionalCancellation
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
