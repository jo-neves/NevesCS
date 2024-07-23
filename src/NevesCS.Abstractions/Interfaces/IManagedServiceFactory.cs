namespace NevesCS.Abstractions.Interfaces
{
    public interface IManagedServiceFactory<out TService>
    {
        public TService Create(string key);
    }
}
