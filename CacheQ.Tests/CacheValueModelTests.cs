using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace CacheQ.Tests
{
    [ExcludeFromCodeCoverage]
    public class CacheValueModelTests
    {
        [Fact]
        public void Test1()
        {
            var cacheValueModel = new CacheValueModel<int>(45);

            var json = JsonSerializer.Serialize(cacheValueModel);
            var deserialized = JsonSerializer.Deserialize<CacheValueModel<int>>(json);

            Assert.Equal(cacheValueModel.Item, deserialized.Item);
        }
    }
}
