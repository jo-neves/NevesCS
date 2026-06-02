namespace NevesCS.Abstractions.Traits
{
    public interface IConvertibleToDictionary<TKey, TValue>
        where TKey : notnull
    {
        public Dictionary<TKey, TValue> ToDictionary();
    }
}
