using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DistributedCache.Extensions;

public static class DistributedCacheSerializerExtensions
{
    /// <summary>
    /// Retrieves a serialized object stored in a distributed cache using a specified key
    /// and deserializes it to the specified generic type.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize to.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <returns>
    /// A deserialized object of the generic type if the key exists in the cache; otherwise, it returns null.
    /// </returns>
    public static T? Get<T>(this IDistributedCache cache, string key) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);

        var value = cache.Get(key);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>
    /// Retrieves a serialized object stored in a distributed cache using a specified key
    /// and deserializes it to the specified generic type.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize to.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="cancellationToken">(Optional) A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A deserialized object of the generic type if the key exists in the cache; otherwise, it returns null.
    /// </returns>
    public static async ValueTask<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);
        
        var value = await cache.GetAsync(key, cancellationToken).ConfigureAwait(false);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>
    /// Serializes and stores an object of the specified type in a distributed cache with a specified key.
    /// </summary>
    /// <typeparam name="T">The type of object to be serialized and stored.</typeparam>
    /// <param name="cache">The instance of the distributed cache to store the object in.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="value">The object to be serialized and stored in the cache.</param>
    public static void Set<T>(this IDistributedCache cache, string key, T value) where T : class
    {
        cache.Set(key, value, new DistributedCacheEntryOptions());
    }

    /// <summary>
    /// Serializes and stores an object of the specified type in a distributed cache with a specified key.
    /// </summary>
    /// <typeparam name="T">The type of object to be serialized and stored.</typeparam>
    /// <param name="cache">The instance of the distributed cache to store the object in.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="value">The object to be serialized and stored in the cache.</param>
    /// <param name="options">
    /// The cache entry options that control caching behaviors such as expiration.
    /// </param>
    public static void Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var serialized = JsonSerializer.Serialize(value);

        cache.SetString(key, serialized, options);
    }


    /// <summary>
    /// Serializes and stores an object of the specified type in a distributed cache with a specified key.
    /// </summary>
    /// <typeparam name="T">The type of object to be serialized and stored.</typeparam>
    /// <param name="cache">The instance of the distributed cache to store the object in.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="value">The object to be serialized and stored in the cache.</param>
    /// <param name="cancellationToken">(Optional) A cancellation token to cancel the asynchronous operation.</param>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        return cache.SetAsync(key, value, new DistributedCacheEntryOptions(), cancellationToken);
    }

    /// <summary>
    /// Asynchronously serializes and stores an object of the specified type in a distributed cache
    /// with a specified key and optional cache entry options.
    /// </summary>
    /// <typeparam name="T">The type of object to be serialized and stored.</typeparam>
    /// <param name="cache">The instance of the distributed cache to store the object in.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="value">The object to be serialized and stored in the cache.</param>
    /// <param name="options">
    /// The cache entry options that control caching behaviors such as expiration.
    /// </param>
    /// <param name="cancellationToken">(Optional) A cancellation token to cancel the asynchronous operation.</param>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var serialized = JsonSerializer.Serialize(value);

        return cache.SetStringAsync(key, serialized, options, cancellationToken);
    }

    /// <summary>
    /// Retrieves an object of the specified type from a distributed cache with a specified key.
    /// If the object is not found in the cache, it invokes a provided delegate to fetch the object,
    /// serializes and stores it in the cache, and then returns it.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve and cache.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="getMethod">
    /// A delegate that provides the method to fetch and return the object if it's not found in the cache.
    /// </param>
    /// <returns>
    /// The retrieved or newly fetched object of the specified type from the cache.
    /// </returns>
    public static T? Get<T>(this IDistributedCache cache, string key, Func<T?> getMethod) where T : class
    {
        return cache.Get<T>(key, getMethod, new DistributedCacheEntryOptions());
    }

    /// <summary>
    /// Retrieves an object of the specified type from a distributed cache with a specified key.
    /// If the object is not found in the cache, it invokes a provided delegate to fetch the object,
    /// serializes and stores it in the cache, and then returns it.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve and cache.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="getMethod">
    /// A delegate that provides the method to fetch and return the object if it's not found in the cache.
    /// </param>
    /// <returns>
    /// The retrieved or newly fetched object of the specified type from the cache.
    /// </returns>
    public static T? Get<T>(this IDistributedCache cache, string key, Func<T?> getMethod, DistributedCacheEntryOptions options) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(getMethod);

        var value = cache.Get<T>(key);
        if (value is null)
        {
            value = getMethod();

            if (value is not null)
            {
                cache.Set(key, value, options);
            }
        }
        return value;
    }


    /// <summary>
    /// Retrieves an object of the specified type from a distributed cache with a specified key.
    /// If the object is not found in the cache, it invokes a provided delegate to fetch the object,
    /// serializes and stores it in the cache, and then returns it.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve and cache.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="getMethod">
    /// A delegate that provides the method to fetch and return the object if it's not found in the cache.
    /// </param>
    /// <returns>
    /// The retrieved or newly fetched object of the specified type from the cache.
    /// </returns>
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> getMethod, CancellationToken cancellationToken = default) where T : class
    {
        return await cache.GetAsync(key, getMethod, new DistributedCacheEntryOptions(), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves an object of the specified type from a distributed cache with a specified key.
    /// If the object is not found in the cache, it invokes a provided delegate to fetch the object,
    /// serializes and stores it in the cache, and then returns it.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve and cache.</typeparam>
    /// <param name="cache">The instance of the distributed cache to query.</param>
    /// <param name="key">The key that identifies the object in the cache.</param>
    /// <param name="getMethod">
    /// A delegate that provides the method to fetch and return the object if it's not found in the cache.
    /// </param>
    /// <returns>
    /// The retrieved or newly fetched object of the specified type from the cache.
    /// </returns>
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> getMethod, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(getMethod);

        var value = await cache.GetAsync<T>(key, cancellationToken).ConfigureAwait(false);

        if (value is null)
        {
            value = await getMethod().ConfigureAwait(false);

            if (value is not null)
            {
                await cache.SetAsync(key, value, options, cancellationToken).ConfigureAwait(false);
            }
        }
        return value;
    }
}
