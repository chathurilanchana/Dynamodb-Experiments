using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MovieRank.IntegrationTests.Setup
{
    // Use more about factory class in https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            // Add mock/test services to the builder here
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
                    var credentials = new BasicAWSCredentials("fakeMyKeyId", "fakeSecretAccessKey");
                    return new AmazonDynamoDBClient(credentials, clientConfig);
                });
                services.AddSingleton<IDynamoDBContext, DynamoDBContext>((serviceProvider) =>
                {
                    IAmazonDynamoDB amazonDynamoDBClient = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
                    DynamoDBContextConfig dynamoDBContextConfig = new DynamoDBContextConfig
                    {
                        // TableNamePrefix = "Movies"
                    };
                    return new DynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
                });
            });

            return base.CreateHost(builder);
        }
    }
}
