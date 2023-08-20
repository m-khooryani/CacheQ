using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Internal;

namespace CacheQ.Tests.E2E;

[ExcludeFromCodeCoverage]
class CustomClock : ISystemClock
{
    public static DateTimeOffset DateTimeOffset = DateTimeOffset.UtcNow;
    public DateTimeOffset UtcNow => DateTimeOffset;
}
