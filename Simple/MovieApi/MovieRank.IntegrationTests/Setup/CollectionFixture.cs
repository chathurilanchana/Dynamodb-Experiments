using Xunit;

namespace MovieRank.IntegrationTests.Setup
{
    // This will be initialsed once for all the test classes that use this collection only once, and disposed only once when all the tests has run.
    //[Collection("Database collection")]
    //public class DatabaseTestClass1
    //{
    //    DatabaseFixture fixture;

    //    public DatabaseTestClass1(DatabaseFixture fixture)
    //    {
    //        this.fixture = fixture;
    //    }
    //}

    //[Collection("Database collection")]
    //public class DatabaseTestClass2
    //{
    //    // ...
    //}
    [CollectionDefinition("api")]
    public class CollectionFixture : ICollectionFixture<TestContext>
    {
    }
}
