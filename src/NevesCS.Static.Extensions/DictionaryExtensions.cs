using NevesCS.Static.Utils;

using System.Collections.Concurrent;

namespace NevesCS.Static.Extensions;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> CloneIntoNew<TKey, TValue>(this IDictionary<TKey, TValue> target)
        where TKey : notnull
    {
        return DictionaryUtils.CloneIntoNew(target);
    }

    public static TValue? GetValueOr<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        TKey key,
        TValue? defaultValue = default)
    {
        return DictionaryUtils.GetValueOr(target, key, defaultValue);
    }

    /// <summary>
    /// Gets an item by key, or adds a new <typeparamref name="TValue"/> instance.
    ///
    /// </summary>
    public static TValue GetOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        TKey key)

        where TValue : new()
    {
        return DictionaryUtils.GetOrCreate(target, key);
    }

    /// <summary>
    /// Gets an item by key, or adds a new value from the <paramref name="valueFactory"/>.
    ///
    /// </summary>
    public static TValue GetOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        TKey key,
        Func<TValue> valueFactory)
    {
        return DictionaryUtils.GetOrCreate(target, key, valueFactory);
    }

    /// <summary>
    /// Get an item by key, or adds a new return value from the <paramref name="asyncValueFactory"/>.
    ///
    /// </summary>
    public static async Task<TValue> GetOrCreateAsync<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        TKey key,
        Func<Task<TValue>> asyncValueFactory)
    {
        return await DictionaryUtils.GetOrCreateAsync(target, key, asyncValueFactory);
    }

    public static IDictionary<TKey, TValue> Upsert<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        TKey key,
        TValue value)

        where TValue : new()
    {
        return DictionaryUtils.Upsert(target, key, value);
    }

    public static void UpdateOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> target,
        IDictionary<TKey, TValue> source)
    {
        DictionaryUtils.UpdateOrCreate(target, source);
    }

    public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> source)

        where TKey : notnull
    {
        return DictionaryUtils.ToConcurrentDictionary(source);
    }
}
