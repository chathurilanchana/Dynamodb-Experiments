using System.Threading.Tasks;
using Xunit;

namespace MovieRank.IntegrationTests.Setup
{
    public class TestContext : IAsyncLifetime
    {
        public Task DisposeAsync()
        {
            //dispose logic here
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            // initialize logic goes here
            return Task.CompletedTask;
        }
    }
}
