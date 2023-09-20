using FakeItEasy;
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
        var service = A.Fake<IFakeService>();

        var cache = _distributedCache.Get<User>(nameof(Get_WhenCacheExist_ReturnsSerializedObject), service.GetOneUser);

        Assert.NotNull(cache);
        A.CallTo(() => service.GetOneUser()).MustHaveHappened();
    }

    [Fact]
    public void Get_WhenCacheAlreadyExist_CallDelegateMethodOnce()
    {
        var service = A.Fake<IFakeService>();

        var cache = _distributedCache.Get<User>(nameof(Get_WhenCacheExist_ReturnsSerializedObject), service.GetOneUser);
        var cache2 = _distributedCache.Get<User>(nameof(Get_WhenCacheExist_ReturnsSerializedObject), service.GetOneUser);

        Assert.NotNull(cache);
        Assert.NotNull(cache2);
        A.CallTo(() => service.GetOneUser()).MustHaveHappenedOnceExactly();
    }
}