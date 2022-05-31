using MovieRank.Domain.Mappers;
using MovieRank.Infrastructure.Repositories;
using MovieRank.Modals;

namespace MovieRank.Domain
{
    public class MovieRankService : IMovieRankService
    {
        private readonly IMovieRankRepository _movieRankRepository;

        public MovieRankService(IMovieRankRepository movieRankRepository)
        {
            _movieRankRepository = movieRankRepository;
        }

        public async Task AddMovieAsync(MovieRankRequest request)
        {
            var mappedMovie = MovieRankMapper.ToDb(request);

            await _movieRankRepository.AddAsync(mappedMovie);
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMoviesFromDatabaseAsync()
        {
            var result = await _movieRankRepository.GetAllItemsFromDatabaseAsync();

            return MovieRankMapper.ToApplication(result);
        }

        public async Task<IEnumerable<MovieResponse>> GetMoviesBeginsWithAsyhnc(int userId, string prefix)
        {
            var result = await _movieRankRepository.GetMoviesBeginsWithAsync(userId, prefix);

            return MovieRankMapper.ToApplication(result);
        }

        public async Task<double> GetRankingAsync(string movieName)
        {
            var result = await _movieRankRepository.GetAllRanksAsync(movieName);

            var overall = result.Average(p => p.Rank);

            return Math.Round(overall, 2);
        }

        public async Task<double> GetRankingForMoviesWithdescriptionPrefixAsync(string movieName, string prefix)
        {
            var result = await _movieRankRepository.GetAllRanksForMoviesWithDescriptionPrefixAsync(movieName,prefix);

            var overall = result.Average(p => p.Rank);

            return Math.Round(overall, 2);
        }

        public async Task<MovieResponse> GetSpecificMovieAsync(int userId, string movieName)
        {
            var result = await _movieRankRepository.GetMovieAsync(userId, movieName);

            return MovieRankMapper.ToApplication(result);
        }

        public async Task UpdateMovieAsync(int userId, UpdateMovieRankRequest request)
        {
            var original = await _movieRankRepository.GetMovieAsync(userId, request.MovieName);
            var mappedMovie = MovieRankMapper.ToDb(userId, original, request);

            await _movieRankRepository.UpdateAsync(mappedMovie);
        }
    }
}
