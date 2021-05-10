using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CacheQ.Tests.E2E.Queries;
using Xunit;

namespace CacheQ.Tests.E2E
{
    [ExcludeFromCodeCoverage]
    [Collection("Tests collection")]
    public class UnitTest1
    {
        private readonly TestFixture _testFixture;

        public UnitTest1(TestFixture fixture)
        {
            _testFixture = fixture;
        }

        [Fact]
        public async Task Test1()
        {
            _testFixture.Reset();

            var query = new EvenNumbersQuery()
            {
                StartRange = 1,
                EndRange = 1000
            };

            await _testFixture.ExecuteQueryAsync(query);
            Assert.Equal(1, EvenNumbersQueryHandler.Calculated); 

            await _testFixture.ExecuteQueryAsync(query);
            Assert.Equal(1, EvenNumbersQueryHandler.Calculated);
        }

        [Fact]
        public async Task Test11()
        {
            _testFixture.Reset();

            var query = new EvenNumbersQuery()
            {
                StartRange = 1,
                EndRange = 1000
            };

            await _testFixture.ExecuteQueryAsync(query);
            Assert.Equal(1, EvenNumbersQueryHandler.Calculated);

            var otherQuery = new EvenNumbersQuery()
            {
                StartRange = 2,
                EndRange = 1000
            };

            await _testFixture.ExecuteQueryAsync(otherQuery);
            Assert.Equal(2, EvenNumbersQueryHandler.Calculated);
        }

        [Fact]
        public async Task Test2()
        {
            _testFixture.Reset();

            var query = new EvenNumbersQuery()
            {
                StartRange = 1,
                EndRange = 1000
            };

            await _testFixture.ExecuteQueryAsync(query);
            Assert.Equal(1, EvenNumbersQueryHandler.Calculated);

            _testFixture.PassLongTime();

            await _testFixture.ExecuteQueryAsync(query);
            Assert.Equal(2, EvenNumbersQueryHandler.Calculated);
        }
    }
}
