using MovieRank.Modals;

namespace MovieRank.Domain
{
    public interface IMovieRankService
    {
        Task<IEnumerable<MovieResponse>> GetAllMoviesFromDatabaseAsync();
        Task<MovieResponse> GetSpecificMovieAsync(int userId, string movieName);
        Task<IEnumerable<MovieResponse>> GetMoviesBeginsWithAsyhnc(int userId, string prefix);
        Task AddMovieAsync(MovieRankRequest request);
        Task UpdateMovieAsync(int userId, UpdateMovieRankRequest request);
        Task<double> GetRankingAsync(string movieName);
        Task<double> GetRankingForMoviesWithdescriptionPrefixAsync(string movieName, string prefix);
    }
}
