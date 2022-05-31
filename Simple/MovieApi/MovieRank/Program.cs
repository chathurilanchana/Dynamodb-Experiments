using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MovieRank.Domain;
using MovieRank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions
{
    Region = Amazon.RegionEndpoint.EUNorth1,
    Profile = "chathuri"
});
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>((serviceProvider) =>
{
    IAmazonDynamoDB amazonDynamoDBClient = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
    DynamoDBContextConfig dynamoDBContextConfig = new DynamoDBContextConfig
    {
       // TableNamePrefix = "Movies"
    };
    return new DynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
});

builder.Services.AddTransient<IMovieRankService,MovieRankService>();
builder.Services.AddTransient<IMovieRankWithDocumentDbRepository, MovieRankWithDocumentDbRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
