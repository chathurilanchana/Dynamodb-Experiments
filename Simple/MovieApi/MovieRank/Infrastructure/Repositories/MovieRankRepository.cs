using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Infrastructure.Models;

namespace MovieRank.Infrastructure.Repositories
{
    public class MovieRankRepository : IMovieRankRepository
    {
        private readonly IDynamoDBContext _dynamodbContext;

        public MovieRankRepository(IDynamoDBContext dynamodbContext)
        {
            _dynamodbContext = dynamodbContext;
        }

        public async Task AddAsync(MovieDb mappedMovie)
        {
            await _dynamodbContext.SaveAsync<MovieDb>(mappedMovie);
        }

        // scan all items
        public async Task<IEnumerable<MovieDb>> GetAllItemsFromDatabaseAsync()
        {
            return await _dynamodbContext.ScanAsync<MovieDb>(new List<ScanCondition>()).GetRemainingAsync();
        }

        //secondary index with secondary index partition key search
        public async Task<IEnumerable<MovieDb>> GetAllRanksAsync(string movieName)
        {
            var config = new DynamoDBOperationConfig
            {
                IndexName = "MovieName-index"
            };
            return await _dynamodbContext.QueryAsync<MovieDb>(movieName, config).GetRemainingAsync();
        }

        //secondary index with its partition key and range key as a prefix
        public async Task<IEnumerable<MovieDb>> GetAllRanksForMoviesWithDescriptionPrefixAsync(string movieName, string prefix)
        {

            var config = new DynamoDBOperationConfig
            {
                IndexName = "Name-Description",
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("Description", ScanOperator.BeginsWith, prefix)
                }
            };

            return await _dynamodbContext.QueryAsync<MovieDb>(movieName, config).GetRemainingAsync();
        }

        //Use Load whn you want to load exactly one item. Same as query with equals scan condition, But LoadAsync is more readable
        public async Task<MovieDb> GetMovieAsync(int userId, string movieName)
        {
            return await _dynamodbContext.LoadAsync<MovieDb>(userId, movieName); // If you have a partition key and a sort key, you must provide both here. Otherwise you will get an exception
            //var config = new DynamoDBOperationConfig
            //{
            //    QueryFilter = new List<ScanCondition>
            //    {
            //        new ScanCondition("MovieName", ScanOperator.Equal, movieName)
            //    }
            //};
            //return (await _dynamodbContext.QueryAsync<MovieDb>(userId, config).GetRemainingAsync()).FirstOrDefault();
        }


        //primary index with its partition key and range key as a prefix
        public async Task<IEnumerable<MovieDb>> GetMoviesBeginsWithAsync(int userId, string prefix)
        {
            var config = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("MovieName", ScanOperator.BeginsWith, prefix)
                }
            };
            return await _dynamodbContext.QueryAsync<MovieDb>(userId, config).GetRemainingAsync(); // here u provide hash key and a condition on sort key to define which items to scan
        }

        public async Task UpdateAsync(MovieDb mappedMovie)
        {
            await _dynamodbContext.SaveAsync<MovieDb>(mappedMovie);
        }
    }
}
