using System.Collections.Concurrent;

namespace NevesCS.Static.Utils;

public static class DictionaryUtils
{
    public static IDictionary<TKey, TValue> CloneIntoNew<TKey, TValue>(IDictionary<TKey, TValue> target)
        where TKey : notnull
    {
        return new Dictionary<TKey, TValue>(target);
    }

    public static IDictionary<TKey, TValue> Upsert<TKey, TValue>(IDictionary<TKey, TValue> target, TKey key, TValue value)
        where TValue : new()
    {
        target[key] = value;

        return target;
    }

    public static TValue? GetValueOr<TKey, TValue>(
        IDictionary<TKey, TValue> target,
        TKey key,
        TValue? defaultValue = default)
    {
        if (!target.TryGetValue(key, out TValue? existingValue))
        {
            return defaultValue;
        }

        return existingValue;
    }

    /// <summary>
    /// Gets an item by key, or adds a new <typeparamref name="TValue"/> instance.
    ///
    /// </summary>
    public static TValue GetOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> target,
        TKey key)
        where TValue : new()
    {
        if (!target.TryGetValue(key, out TValue? value))
        {
            value = new TValue();
            target.Add(key, value);
        }

        return value;
    }

    /// <summary>
    /// Gets an item by key, or adds a new value from the <paramref name="valueFactory"/>.
    ///
    /// </summary>
    public static TValue GetOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> target,
        TKey key,
        Func<TValue> valueFactory)
    {
        if (!target.TryGetValue(key, out TValue? value))
        {
            value = valueFactory();
            target.Add(key, value);
        }

        return value;
    }

    /// <summary>
    /// Get an item by key, or adds a new return value from the <paramref name="asyncValueFactory"/>.
    ///
    /// </summary>
    public static async Task<TValue> GetOrCreateAsync<TKey, TValue>(
        IDictionary<TKey, TValue> target,
        TKey key,
        Func<Task<TValue>> asyncValueFactory)
    {
        if (!target.TryGetValue(key, out TValue? existingValue))
        {
            existingValue = await asyncValueFactory();
            target.Add(key, existingValue);
        }

        return existingValue;
    }

    public static void UpdateOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> target,
        IDictionary<TKey, TValue> source)
    {
        foreach (var kvp in source)
        {
            target[kvp.Key] = kvp.Value;
        }
    }

    public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(
        IDictionary<TKey, TValue> source)

        where TKey : notnull
    {
        return new ConcurrentDictionary<TKey, TValue>(source);
    }
}
