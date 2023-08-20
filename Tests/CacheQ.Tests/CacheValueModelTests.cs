using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace CacheQ.Tests;

[ExcludeFromCodeCoverage]
public class CacheValueModelTests
{
    [Fact]
    public void Test1()
    {
        var cacheValueModel = 45;

        var json = JsonSerializer.Serialize(cacheValueModel);
        var deserialized = JsonSerializer.Deserialize<int>(json);

        Assert.Equal(cacheValueModel, deserialized);
    }
}
