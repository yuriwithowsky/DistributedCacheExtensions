using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Extensions.Tests;

public class Tests
{
    private readonly IDistributedCache _distributedCache;

    public Tests()
    {
        _distributedCache = new FakeDistributedCache();
    }

    [Fact]
    public void Get_WhenCacheNotExist_ReturnsNull()
    {
        var user = new User("John", "Snow");

        var cache = _distributedCache.Get<User>(nameof(Get_WhenCacheNotExist_ReturnsNull));

        Assert.Null(cache);
    }

    [Fact]
    public void Get_WhenCacheExist_ReturnsSerializedObject()
    {
        var user = new User("John", "Snow");

        _distributedCache.Set(nameof(Get_WhenCacheExist_ReturnsSerializedObject), user);

        var cache = _distributedCache.Get<User>(nameof(Get_WhenCacheExist_ReturnsSerializedObject));

        Assert.NotNull(cache);
    }

    [Fact]
    public void Get_WhenCacheNotExist_ReturnsSerializedObject()
    {
        User getUser ()
        {
            return new User("John", "Snow");
        };

        var cache = _distributedCache.Get<User>(nameof(Get_WhenCacheExist_ReturnsSerializedObject), getUser);

        Assert.NotNull(cache);
    }
}

public record User(string FirstName, string LastName);