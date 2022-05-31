using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Infrastructure.Models;

namespace MovieRank.Infrastructure.Repositories
{
    public interface IMovieRankWithDocumentDbRepository
    {
        Task<IEnumerable<Document>> GetAllItemsFromDatabaseAsync();
        Task<Document> GetMovieAsync(int userId, string movieName);
        Task<IEnumerable<Document>> GetMoviesBeginsWithAsync(int userId, string prefix);
        Task AddAsync(Document mappedMovie);
        Task UpdateAsync(Document mappedMovie);
        Task<IEnumerable<Document>> GetAllRanksAsync(string movieName);
        Task<IEnumerable<Document>> GetAllRanksForMoviesWithDescriptionPrefixAsync(string movieName, string prefix);
    }
}
