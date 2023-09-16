using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DistributedCache.Extensions;

public static class DistributedCacheSerializerExtensions
{
    public static T? Get<T>(this IDistributedCache cache, string key) where T : class
    {
        var value = cache.Get(key);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }
    public static async ValueTask<T?> GetAsync<T>(this IDistributedCache cache, string key) where T : class
    {
        var value = await cache.GetAsync(key).ConfigureAwait(false);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    public static void Set<T>(this IDistributedCache cache, string key, T value) where T : class
    {
        var serialized = JsonSerializer.Serialize(value);

        cache.SetString(key, serialized);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value) where T : class
    {
        var serialized = JsonSerializer.Serialize(value);

        await cache.SetStringAsync(key, serialized).ConfigureAwait(false);
    }

    public static T? Get<T>(IDistributedCache cache, string key, Func<T> getMethod) where T : class
    {
        var value = cache.Get<T>(key);
        if (value is null)
        {
            value = getMethod();

            if (value is not null)
            {
                cache.Set(key, value);
            }
        }
        return value;
    }

    public static async Task<T?> GetAsync<T>(IDistributedCache cache, string key, Func<Task<T>> getMethod) where T : class
    {
        var value = await cache.GetAsync<T>(key).ConfigureAwait(false);

        if (value is null)
        {
            value = await getMethod().ConfigureAwait(false);

            if (value is not null)
            {
                await cache.SetAsync(key, value).ConfigureAwait(false);
            }
        }
        return value;
    }
}
