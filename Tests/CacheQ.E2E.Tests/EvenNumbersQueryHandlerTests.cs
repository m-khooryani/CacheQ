using System.Diagnostics.CodeAnalysis;
using CacheQ.Tests.E2E.Queries;
using Xunit;

namespace CacheQ.Tests.E2E;

[ExcludeFromCodeCoverage]
[Collection("Tests collection")]
public class EvenNumbersQueryHandlerTests
{
    private readonly TestFixture _testFixture;

    public EvenNumbersQueryHandlerTests(TestFixture fixture)
    {
        _testFixture = fixture;
    }

    [Fact]
    public async Task Given_InitialExecution_When_QueryExecuted_Then_HandlerExecutedOnce()
    {
        _testFixture.Reset();

        var query = new EvenNumbersQuery()
        {
            StartRange = 1,
            EndRange = 1000
        };

        await _testFixture.ExecuteQueryAsync(query);
        Assert.Equal(1, EvenNumbersQueryHandler.ExecutedCount);

        await _testFixture.ExecuteQueryAsync(query);
        Assert.Equal(1, EvenNumbersQueryHandler.ExecutedCount);
    }

    [Fact]
    public async Task Given_InitialExecution_When_DifferentQueriesExecuted_Then_HandlerExecutedTwice()
    {
        _testFixture.Reset();

        var query = new EvenNumbersQuery()
        {
            StartRange = 1,
            EndRange = 1000
        };

        await _testFixture.ExecuteQueryAsync(query);
        Assert.Equal(1, EvenNumbersQueryHandler.ExecutedCount);

        var otherQuery = new EvenNumbersQuery()
        {
            StartRange = 2,
            EndRange = 1000
        };

        await _testFixture.ExecuteQueryAsync(otherQuery);
        Assert.Equal(2, EvenNumbersQueryHandler.ExecutedCount);
    }

    [Fact]
    public async Task Given_TimePassed_When_SameQueryExecuted_Then_HandlerExecutedTwice()
    {
        _testFixture.Reset();

        var query = new EvenNumbersQuery()
        {
            StartRange = 1,
            EndRange = 1000
        };

        await _testFixture.ExecuteQueryAsync(query);
        Assert.Equal(1, EvenNumbersQueryHandler.ExecutedCount);

        _testFixture.PassLongTime();

        await _testFixture.ExecuteQueryAsync(query);
        Assert.Equal(2, EvenNumbersQueryHandler.ExecutedCount);
    }
}
