namespace NevesCS.Abstractions.Traits
{
    public interface IAsyncInitializableWithCancellation
    {
        public Task InitializeAsync(CancellationToken cancellationToken);
    }
}
