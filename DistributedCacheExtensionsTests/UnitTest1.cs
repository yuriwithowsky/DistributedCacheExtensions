using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Extensions.Tests;

public class UnitTest1
{
    private readonly IDistributedCache _distributedCache;

    public UnitTest1()
    {
        _distributedCache = new FakeDistributedCache();
    }

    [Fact]
    public void Test1_WhenCacheNotExist()
    {
        var user = new User("John", "Snow");

        var cache = _distributedCache.Get<User>(nameof(Test1_WhenCacheNotExist));

        Assert.Null(cache);
    }

    [Fact]
    public void Test1_WhenCacheExist()
    {
        var user = new User("John", "Snow");

        _distributedCache.Set(nameof(Test1_WhenCacheExist), user);

        var cache = _distributedCache.Get<User>(nameof(Test1_WhenCacheExist));

        Assert.NotNull(cache);
    }
}

public record User(string firstName, string lastName);