using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Infrastructure.Models;

namespace MovieRank.Infrastructure.Repositories
{
    //This uses dynamodb's object persistence model
    public class MovieRankWithDocumentDbRepository : IMovieRankWithDocumentDbRepository
    {
        private const string TableName = "Movies";
        private readonly Table _table;

        public MovieRankWithDocumentDbRepository(IAmazonDynamoDB amazonDynamoDbClient)
        {
            _table = Table.LoadTable(amazonDynamoDbClient, TableName);
        }

        public async Task AddAsync(Document mappedMovie)
        {
            await _table.PutItemAsync(mappedMovie);
        }

        public async Task<IEnumerable<Document>> GetAllItemsFromDatabaseAsync()
        {

            var scanOperationConfig = new ScanOperationConfig();
            var result = await _table.Scan(scanOperationConfig).GetRemainingAsync();

            return result;
        }

        public async Task<IEnumerable<Document>> GetAllRanksAsync(string movieName)
        {
            var filter = new QueryFilter("MovieName", QueryOperator.Equal, movieName);

            var queryOperationConfig = new QueryOperationConfig
            {
                IndexName = "MovieName-index",
                Filter = filter
            };

            return await _table.Query(queryOperationConfig).GetRemainingAsync();
        }

        public async Task<IEnumerable<Document>> GetAllRanksForMoviesWithDescriptionPrefixAsync(string movieName, string prefix)
        {
            var filter = new QueryFilter("MovieName", QueryOperator.Equal, movieName);
            filter.AddCondition("Description", QueryOperator.BeginsWith, prefix);

            var queryOperationConfig = new QueryOperationConfig
            {
                IndexName = "Name-Description",
                Filter = filter
            };

            return await _table.Query(queryOperationConfig).GetRemainingAsync();
        }

        public async Task<Document> GetMovieAsync(int userId, string movieName)
        {
            return await _table.GetItemAsync(userId, movieName);    
        }

        public async Task<IEnumerable<Document>> GetMoviesBeginsWithAsync(int userId, string prefix)
        {
            var filter = new QueryFilter("UserId",QueryOperator.Equal, userId);
            filter.AddCondition("MovieName", QueryOperator.BeginsWith, prefix);

            return await _table.Query(filter).GetRemainingAsync();
            // try below 2 as well
            //return await _table.Query(userId, new QueryFilter("MovieName", QueryOperator.BeginsWith, prefix)).GetRemainingAsync();
        }

        public async Task UpdateAsync(Document mappedMovie)
        {
            await _table.UpdateItemAsync(mappedMovie);
        }
    }
}
