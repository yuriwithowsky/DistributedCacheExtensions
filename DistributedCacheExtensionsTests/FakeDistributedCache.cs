using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Extensions.Tests;
public class FakeDistributedCache : IDistributedCache
{
    private Dictionary<string, byte[]> _cache = new Dictionary<string, byte[]>();

    public byte[]? Get(string key) =>
        _cache.GetValueOrDefault(key);

    public Task<byte[]?> GetAsync(string key, CancellationToken token = default) =>
        Task.FromResult(_cache.GetValueOrDefault(key));

    public void Refresh(string key)
    {
        throw new System.NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key) =>
        _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default) =>
        Task.FromResult(_cache.Remove(key));

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options) =>
        _cache.Add(key, value);

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        _cache.Add(key, value);
        return Task.CompletedTask;
    }
}