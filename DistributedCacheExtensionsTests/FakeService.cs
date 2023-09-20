namespace DistributedCache.Extensions.Tests;

public interface IFakeService
{
    User GetOneUser();
}

public class FakeService : IFakeService
{
    public User GetOneUser() => new("John", "Snow");
}

public record User(string FirstName, string LastName);
