using MovieRank.Infrastructure.Models;

namespace MovieRank.Infrastructure.Repositories
{
    public interface IMovieRankRepository
    {
        Task<IEnumerable<MovieDb>> GetAllItemsFromDatabaseAsync();
        Task<MovieDb> GetMovieAsync(int userId, string movieName);
        Task<IEnumerable<MovieDb>> GetMoviesBeginsWithAsync(int userId, string prefix);
        Task AddAsync(MovieDb mappedMovie);
        Task UpdateAsync(MovieDb mappedMovie);
        Task<IEnumerable<MovieDb>> GetAllRanksAsync(string movieName);
        Task<IEnumerable<MovieDb>> GetAllRanksForMoviesWithDescriptionPrefixAsync(string movieName,string prefix);
    }
}
