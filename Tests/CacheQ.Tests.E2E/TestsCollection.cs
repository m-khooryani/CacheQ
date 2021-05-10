using Xunit;

namespace CacheQ.Tests.E2E
{
    [CollectionDefinition("Tests collection")]
    public class TestsCollection : ICollectionFixture<TestFixture>
    {
    }
}
